#region License GNU GPL
// AutoFight.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System.Drawing;
using System.Linq;
using System.Timers;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Spells;
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
using BiM.Behaviors.Game.Shortcuts;
using System.Diagnostics;

namespace FightPlugin
{
    internal static class WelcomeMessageRegister
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content == ".fight on")
            {
                message.BlockNetworkSend();// do not send this message to the server

                bot.AddFrame(new AutoFight(bot));
                bot.Character.SendMessage("Auto fight started");
            }
            else if (message.content == ".fight off")
            {
                message.BlockNetworkSend();// do not send this message to the server


                bot.RemoveFrame<AutoFight>();
                bot.Character.SendMessage("Auto fight stopped");

            }
        }
    }


    internal class AutoFight : Frame<AutoFight>
    {
        private PlayedFighter m_character;
        private SimplerTimer m_checkTimer;
        private bool m_sit = false;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ContextActor.MoveStopHandler m_stopMovingDelegate;

        public AutoFight(Bot bot)
            : base (bot)
        {
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
                Bot.Character.Say("/sit");

                m_sit = true;

                Bot.Character.StartMoving += StandUp;
            }
        }

        private void StandUp(ContextActor sender, MovementBehavior path)
        {
            m_sit = false;
            Bot.Character.StartMoving -= StandUp;
        }


        private void CheckMonsters()
        {
            if ((Bot.Character.Stats.Health * 3) < Bot.Character.Stats.MaxHealth)
            {
                if (!m_sit)
                {
                    Bot.Character.SendMessage(String.Format("Character health too low : {0}/{1}", Bot.Character.Stats.Health, Bot.Character.Stats.MaxHealth));

                    Bot.CallDelayed(500, Sit);
                }

                return;
            }

            var monster = Bot.Character.Map.Actors.OfType<GroupMonster>()
                .Where(x => x.Level < Bot.Character.Level * 2)
                .OrderBy(x => x.Level).FirstOrDefault();

            if (monster != null)
            {
                Bot.Character.TryStartFightWith(monster);
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
                Bot.CallDelayed(500, PlaceToWeakestEnemy);

                Bot.CallDelayed(2500, new Action(() => Bot.SendToServer(new GameFightReadyMessage(true))));
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
                m_character.PassTurn(); 
            }
            else
            {
                MoveNear(nearestMonster, (int)(m_character.Cell.DistanceTo(nearestMonster.Cell) - m_character.GetRealSpellRange(spell.LevelTemplate)));

               // wait until the movement ends
                if (m_stopMovingDelegate != null)
                {
                    Bot.Character.Fighter.StopMoving -= m_stopMovingDelegate;
                    m_stopMovingDelegate = null;
                }

                m_stopMovingDelegate = (sender, behavior, canceled) => OnStopMoving(spell, nearestMonster);
                Bot.Character.Fighter.StopMoving += m_stopMovingDelegate;
            }

                       
        }

        private void OnStopMoving(Spell spell, Fighter enemy)
        {
            Bot.Character.Fighter.StopMoving -= m_stopMovingDelegate;
            m_stopMovingDelegate = null;

            m_character.CastSpell(spell, enemy.Cell);
            MoveFar();

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

            var cell = Bot.Character.Fighter.Team.PlacementCells.OrderBy(x => x.ManhattanDistanceTo(enemy.Cell)).FirstOrDefault();
            Bot.Character.Fighter.ChangePrePlacement(cell);
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
            var enemies = m_character.GetOpposedTeam().Fighters;

            var shape = new Lozenge(0, (byte) m_character.Stats.CurrentMP);
            var possibleCells = shape.GetCells(m_character.Cell, m_character.Map);
            var orderedCells = from cell in possibleCells
                               where m_character.Fight.IsCellWalkable(cell, false, m_character.Cell)
                               orderby enemies.Sum(x => cell.ManhattanDistanceTo(x.Cell)) descending
                               select cell;

            var dest = orderedCells.FirstOrDefault();

            if (dest == null)
                return;

            m_character.Move(dest);
        }

        private Fighter GetNearestEnemy()
        {
            var enemyTeam = m_character.GetOpposedTeam();

            Fighter nearestFighter = null;
            foreach (var enemy in enemyTeam.Fighters)
            {
                if (!enemy.IsAlive)
                    continue;

                if (nearestFighter == null)
                    nearestFighter = enemy;

                else if (m_character.Cell.ManhattanDistanceTo(enemy.Cell) < 
                    nearestFighter.Cell.ManhattanDistanceTo(m_character.Cell))
                {
                    nearestFighter = enemy;
                }
            }

            return nearestFighter;
        }


        public override void OnAttached()
        {
            base.OnAttached();
        }

        public override void OnDetached()
        {
            if (Bot.Character != null)
            {
                Bot.Character.FightJoined -= OnFightJoined;
                Bot.Character.FightLeft -= OnFightLeft;
                Bot.Character.MapJoined -= OnMapJoined;
            }

            if (m_character != null)
            {
                m_character.TurnStarted -= OnTurnStarted;
                m_character.Fight.StateChanged -= OnStateChanged;
                m_character = null;
            }
        }
    }
}