using System.Drawing;
using System.Linq;
using System.Timers;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Spells.Shapes;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.Core.Threading;
using BiM.Protocol.Messages;
using System;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.Actors;
using NLog;

namespace FightPlugin
{
    internal static class WelcomeMessageRegister
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            // if the client sends ".hello" in the chat
            if (message.content == ".fight on")
            {
                message.BlockNetworkSend();// do not send this message to the server

                bot.AddHandler(new AutoFight(bot));
                bot.Character.SendMessage("Auto fight started");
            }
            else if (message.content == ".fight off")
            {
                message.BlockNetworkSend();// do not send this message to the server


                bot.RemoveHandler<AutoFight>();
                bot.Character.SendMessage("Auto fight stopped");

            }
        }
    }


    internal class AutoFight
    {
        private readonly Bot m_bot;
        private PlayedFighter m_character;
        private SimplerTimer m_checkTimer;
        private bool m_sit = false;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AutoFight(Bot bot)
        {
            m_bot = bot;
            bot.Character.FightJoined += OnFightJoined;
            bot.Character.FightLeft += OnFightLeft;
            bot.Character.MapJoined += OnMapJoined;

            if (bot.Character.IsFighting())
                OnFightJoined(bot.Character, bot.Character.Fight);
            else if (bot.Character.Map != null)
                OnMapJoined(bot.Character, bot.Character.Map);
        }

        private void OnMapJoined(PlayedCharacter character, Map map)
        {
            m_checkTimer = character.Bot.CallPeriodically(4 * 1000, CheckMonsters);
        }

        private void Sit()
        {
            if (!m_sit)
            {
                m_bot.Character.Say("/sit");
                m_sit = true;

                m_bot.Character.StartMoving += StandUp;
            }
        }

        private void StandUp(ContextActor sender, MovementBehavior path)
        {
            m_sit = false;
            m_bot.Character.StartMoving -= StandUp;
        }


        private void CheckMonsters()
        {
            if ((m_bot.Character.Stats.Health * 3) < m_bot.Character.Stats.MaxHealth)
            {
                if (!m_sit)
                {
                    m_bot.Character.SendMessage("Character health too low");

                    m_bot.CallDelayed(500, Sit);
                }

                return;
            }

            var monster = m_bot.Character.Map.Actors.OfType<GroupMonster>()
                .Where(x => x.Level < m_bot.Character.Level * 2)
                .OrderBy(x => x.Level).FirstOrDefault();

            if (monster != null)
            {
                m_bot.Character.TryStartFightWith(monster);
            }
        }

        private void OnFightJoined(PlayedCharacter character, Fight fight)
        {
            if (m_checkTimer != null)
                m_checkTimer.Dispose();

            m_character = character.Fighter;
            character.Fighter.TurnStarted += OnTurnStarted;
            fight.StateChanged += OnStateChanged;
        }

        private void OnStateChanged(Fight fight, FightPhase phase)
        {
            if (phase == FightPhase.Placement)
            {
                m_bot.CallDelayed(500, PlaceToWeakestEnemy);

                m_bot.CallDelayed(2500, new Action(() => m_bot.SendToServer(new GameFightReadyMessage(true))));
            }
        }

        private void OnFightLeft(PlayedCharacter character, Fight fight)
        {
            m_character = null;
            character.Fighter.TurnStarted -= OnTurnStarted;
            fight.StateChanged -= OnStateChanged;
        }

        private void OnTurnStarted(Fighter fighter)
        {
            var bot = BotManager.Instance.GetCurrentBot();

            StartAI();
        }

        private void StartAI()
        {
            var nearestMonster = GetNearestEnemy();
            var shortcut = m_character.Character.SpellShortcuts.Get(1);

            if (shortcut == null)
            {
                m_character.Character.SendMessage("No spell on slot 1");
                return;
            }

            var spell = shortcut.GetSpell();
            if (spell == null)
            {
                m_character.Character.SendMessage("No spell on slot 1");
                return;
            }

            if (m_character.IsInSpellRange(nearestMonster.Cell, spell.LevelTemplate))
            {
                m_character.CastSpell(shortcut.GetSpell(), nearestMonster.Cell);
                MoveFar();
            }
            else
            {
                MoveNear(nearestMonster, (int)( m_character.Cell.DistanceTo(nearestMonster.Cell) - m_character.GetRealSpellRange(spell.LevelTemplate) ));
                m_character.CastSpell(shortcut.GetSpell(), nearestMonster.Cell);
                MoveFar();
            }

            m_character.PassTurn();
        }

        private void PlaceToWeakestEnemy()
        {
            var enemy = m_character.GetOpposedTeam().Fighters.OrderBy(x => x.Level).FirstOrDefault();
            if (enemy == null)
            {
                logger.Warn("PlaceToWeakestEnemy : enemy is null");
                return;
            }

            var cell = m_bot.Character.Fighter.Team.PlacementCells.OrderBy(x => x.DistanceTo(enemy.Cell)).FirstOrDefault();
            m_bot.Character.Fighter.ChangePrePlacement(cell);
        }

        private void MoveNear(Fighter fighter, int mp)
        {
            var dest = fighter.Cell.GetAdjacentCells().OrderBy(cell => cell.DistanceTo(m_character.Cell)).FirstOrDefault();

            if (dest == null)
                return;

            m_character.Move(dest);
        }

        private void MoveFar()
        {
            var ennemies = m_character.GetOpposedTeam().Fighters;

            var shape = new HalfLozenge(0, (byte) m_character.Stats.CurrentMP);
            var possibleCells = shape.GetCells(m_character.Cell, m_character.Map);
            var orderedCells = from cell in possibleCells
                               where m_character.Fight.IsCellWalkable(cell, false, m_character.Cell)
                               orderby ennemies.Sum(x => cell.DistanceTo(x.Cell)) descending
                               select cell;

            var dest = orderedCells.FirstOrDefault();

            if (dest == null)
                return;

            m_character.Move(dest);
        }

        private Fighter GetNearestEnemy()
        {
            var ennemyTeam = m_character.GetOpposedTeam();

            Fighter nearestFighter = null;
            foreach (var ennemy in ennemyTeam.Fighters)
            {
                if (nearestFighter == null)
                    nearestFighter = ennemy;

                else if (m_character.Cell.DistanceTo(ennemy.Cell) < nearestFighter.Cell.DistanceTo(m_character.Cell))
                {
                    nearestFighter = ennemy;
                }
            }

            return nearestFighter;
        }
    }
}