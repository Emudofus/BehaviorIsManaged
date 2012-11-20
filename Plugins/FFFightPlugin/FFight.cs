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
using System;
using System.Linq;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.Spells;
using BiM.Behaviors.Game.Spells.Shapes;
using BiM.Behaviors.Game.World;
using BiM.Core.Messages;
using BiM.Core.Threading;
using BiM.Protocol.Messages;
using NLog;

namespace FightPlugin
{
    internal static class WelcomeMessageRegister
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content == ".FF on")
            {
                message.BlockNetworkSend();// do not send this message to the server                
                bot.AddFrame(new FFight(bot));
                bot.Character.SendMessage("Experimental AI fight started");
            }
            else if (message.content == ".FF off")
            {
                message.BlockNetworkSend();// do not send this message to the server
                bot.RemoveFrame<FFight>();
                bot.Character.SendMessage("Experimental AI fight stopped");
            }
        }
    }


    internal class FFight : Frame<FFight>
    {
        private PlayedFighter _character;
        private SimplerTimer _checkTimer;
        private bool _sit = false;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ContextActor.MoveStopHandler _stopMovingDelegate;
        private Fighter.SpellCastHandler _spellCastedDelegate;

        public FFight(Bot bot)
            : base(bot)
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
            _checkTimer = character.Bot.CallPeriodically(4 * 1000, CheckMonsters);
        }

        private void Sit()
        {
            if (!_sit)
            {
                Bot.SendToServer(new EmotePlayRequestMessage(1));
                //Bot.Character.Say("/sit");

                _sit = true;

                Bot.Character.StartMoving += StandUp;
            }
        }

        private void StandUp(ContextActor sender, MovementBehavior path)
        {
            _sit = false;
            Bot.Character.StartMoving -= StandUp;
        }


        private void CheckMonsters()
        {
            if ((Bot.Character.Stats.Health * 2) < Bot.Character.Stats.MaxHealth)
            {
                if (!_sit)
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
                Bot.Character.SendMessage(String.Format("Trying to start a fight with group lv {0}, cell {1}, leader {2} lv {3}", monster.Level, monster.Cell, monster.Leader.Name, monster.Leader.Grade.grade));
                Bot.Character.TryStartFightWith(monster);
            }
        }

        private void OnFightJoined(PlayedCharacter character, Fight fight)
        {
            if (_checkTimer != null)
                _checkTimer.Dispose();

            _character = character.Fighter;
            character.Fighter.TurnStarted += OnTurnStarted;
            fight.StateChanged += OnStateChanged;
        }

        private void OnStateChanged(Fight fight, FightPhase phase)
        {
            if (phase == FightPhase.Placement)
            {
                Bot.CallDelayed(500, FindOptimalPlacement);

                Bot.CallDelayed(2500, new Action(() => Bot.SendToServer(new GameFightReadyMessage(true))));
            }
        }

        private void OnFightLeft(PlayedCharacter character, Fight fight)
        {            
            _character = null;
            character.Fighter.TurnStarted -= OnTurnStarted;
            fight.StateChanged -= OnStateChanged;
            character.Fighter.SpellCasted -= _spellCastedDelegate;
        }

        private void OnTurnStarted(Fighter fighter)
        {
            var bot = BotManager.Instance.GetCurrentBot();

            StartAI();
        }


        private void StartAI()
        {
            if (_stopMovingDelegate != null)
            {
                _character.StopMoving -= _stopMovingDelegate;
                _stopMovingDelegate = null;
            }
            if (_spellCastedDelegate != null)
            {
                _character.StopMoving -= _stopMovingDelegate;
                _stopMovingDelegate = null;
            }

            var nearestMonster = GetNearestEnemy();
            int maxDistanceWished = -1;
            bool inLine = false;
            foreach (Spell spell in _character.GetOrderListOfSimpleAttackSpells(nearestMonster, true))
            {
                bool inRange = _character.IsInSpellRange(nearestMonster.Cell, spell.LevelTemplate);
                
                if (inRange)
                {
                    _character.CastSpell(spell, nearestMonster.Cell);
                    _spellCastedDelegate = (sender, canceled) => StartAI();
                    _character.SpellCasted += _spellCastedDelegate;
                    return;
                }

                // Available but not in range
                if (_character.GetRealSpellRange(spell.LevelTemplate) > maxDistanceWished)
                {
                    maxDistanceWished = _character.GetRealSpellRange(spell.LevelTemplate);
                    inLine = spell.LevelTemplate.castInLine;
                }
            }
            
            // No other spells can be cast => move away and pass the turn
            if (maxDistanceWished == -1)
            { 
                MoveFar();
                _character.PassTurn();
                return;
            }

            // If no spell is at range, then try to come closer and try again
            MoveNear(nearestMonster, (int)(_character.Cell.ManhattanDistanceTo(nearestMonster.Cell) - maxDistanceWished), inLine);           

            _stopMovingDelegate = (sender, behavior, canceled) => StartAI();
            Bot.Character.Fighter.StopMoving += _stopMovingDelegate;
        }

        private void OnStopMoving(Spell spell, Fighter enemy)
        {
            Bot.Character.Fighter.StopMoving -= _stopMovingDelegate;
            _stopMovingDelegate = null;

            _character.CastSpell(spell, enemy.Cell);
            MoveFar();

            _character.PassTurn();
        }

        private void FindOptimalPlacement()
        {
            Fighter weakestEnemy = FindWeakestEnemy();
            Spell bestSpell = _character.GetOrderListOfSimpleAttackSpells(weakestEnemy, true).FirstOrDefault();
            if (bestSpell == null || weakestEnemy == null)
            {
                logger.Warn("FindOptimalPlacement : can't find a position");
                PlaceToWeakestEnemy();
                return;
            }
            PlaceAtDistanceFromWeakestEnemy(_character.GetRealSpellRange(bestSpell.LevelTemplate));
        }

        private Fighter FindWeakestEnemy()
        {
            return _character.GetOpposedTeam().Fighters.OrderBy(x => x.Level).FirstOrDefault();
        }

        private void PlaceToWeakestEnemy()
        {
            var enemy = _character.GetOpposedTeam().Fighters.OrderBy(x => x.Level).FirstOrDefault();
            if (enemy == null)
            {
                logger.Warn("PlaceToWeakestEnemy : enemy is null");
                return;
            }

            var cell = Bot.Character.Fighter.Team.PlacementCells.OrderBy(x => x.ManhattanDistanceTo(enemy.Cell)).FirstOrDefault();
            Bot.Character.Fighter.ChangePrePlacement(cell);
        }

        private void PlaceAtDistanceFromWeakestEnemy(int distance)
        {
            var weakestEnemy = _character.GetOpposedTeam().Fighters.OrderBy(x => x.Level).FirstOrDefault();
            if (weakestEnemy == null)
            {
                logger.Warn("PlaceToWeakestEnemy : enemy is null");
                return;
            }
            // 1st step : we try to find all cells where weakest ennemy is closest than all other ennemies
            Cell[] startingSet = Bot.Character.Fighter.Team.PlacementCells;
            foreach(Fighter otherEnnemy in _character.GetOpposedTeam().Fighters)
                if (otherEnnemy != weakestEnemy)
                    startingSet = startingSet.Where(x => x.ManhattanDistanceTo(otherEnnemy.Cell) >= x.ManhattanDistanceTo(weakestEnemy.Cell)).ToArray();
            // 2nd step : if none, then we only remove cells where we are in contact of any other ennemy 
            if (startingSet.Length == 0)
            {
                startingSet = Bot.Character.Fighter.Team.PlacementCells;
                foreach (Fighter otherEnnemy in _character.GetOpposedTeam().Fighters)
                    if (otherEnnemy != weakestEnemy)
                        startingSet = startingSet.Where(x => x.ManhattanDistanceTo(otherEnnemy.Cell) > 1).ToArray();
            }

            // 3st step : if still none, just keep all cells. 
            if (startingSet.Length == 0)
                startingSet = Bot.Character.Fighter.Team.PlacementCells;
            
            // Find a cell as far as possible from weakest ennemy, but not over distance
            var cell = startingSet.Where(x => x.ManhattanDistanceTo(weakestEnemy.Cell) <= distance).OrderBy(x => x.ManhattanDistanceTo(weakestEnemy.Cell)).LastOrDefault();

            // If none under distance, then the closest
            if (cell==null)
                cell = startingSet.OrderBy(x => x.ManhattanDistanceTo(weakestEnemy.Cell)).FirstOrDefault();
            Bot.Character.Fighter.ChangePrePlacement(cell);
        }

        private void MoveNear(Fighter fighter, int mp, bool inLine = false)
        {
            Cell dest = null;
            if (inLine)
                _character.Move(
                    Math.Abs(fighter.Cell.X - _character.Cell.X) > Math.Abs(fighter.Cell.Y - _character.Cell.Y)
                        ? _character.Map.Cells[_character.Cell.X, fighter.Cell.Y]
                        : _character.Map.Cells[fighter.Cell.X, _character.Cell.Y], mp);
            else
                // Try to go as close as possible to a cell adjacent with the target
                dest = fighter.Cell.GetAdjacentCells().OrderBy(cell => cell.ManhattanDistanceTo(_character.Cell)).FirstOrDefault();

            if (dest == null)
                return;

            _character.Move(dest, mp);
        }

        private void MoveFar()
        {
            var enemies = _character.GetOpposedTeam().Fighters;

            var shape = new Lozenge(0, (byte)_character.Stats.CurrentMP);
            var possibleCells = shape.GetCells(_character.Cell, _character.Map);
            var orderedCells = from cell in possibleCells
                               where _character.Fight.IsCellWalkable(cell, false, _character.Cell)
                               orderby enemies.Sum(x => cell.ManhattanDistanceTo(x.Cell)) descending
                               select cell;

            var dest = orderedCells.FirstOrDefault();

            if (dest == null)
                return;

            _character.Move(dest);
        }

        private Fighter GetNearestEnemy()
        {
            var enemyTeam = _character.GetOpposedTeam();

            Fighter nearestFighter = null;
            foreach (var enemy in enemyTeam.Fighters)
            {
                if (!enemy.IsAlive)
                    continue;

                if (nearestFighter == null)
                    nearestFighter = enemy;

                else if (_character.Cell.ManhattanDistanceTo(enemy.Cell) <
                    nearestFighter.Cell.ManhattanDistanceTo(_character.Cell))
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

            if (_character != null)
            {
                _character.TurnStarted -= OnTurnStarted;
                _character.Fight.StateChanged -= OnStateChanged;
                _character = null;
            }
        }
    }
}