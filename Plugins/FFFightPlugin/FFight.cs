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

// Author : FastFrench - antispam@laposte.net
#endregion
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Diagnostics;
using BiM.Behaviors.Game.World.Pathfinding;

namespace FightPlugin
{
    internal static class WelcomeMessageRegister
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content.StartsWith(".FF"))
            {
                message.BlockNetworkSend();// do not send this message to the server                
            }

            if (message.content == ".help")
            {
                bot.Character.SendMessage(".FF on => Starts experimental AI fight");
                bot.Character.SendMessage(".FF off => Stops experimental AI fight");
                bot.Character.SendMessage(".FF stats => gives some stats");
            }

            else if (message.content == ".FF on")
            {
                bot.AddFrame(new FFight(bot));
                bot.Character.SendMessage("Experimental AI fight started");
            }
            else if (message.content == ".FF off")
            {
                bot.RemoveFrame<FFight>();
                bot.Character.SendMessage("Experimental AI fight stopped");
            }
            else if (message.content == ".FF stats")
            {
                if (!bot.HasFrame<FFight>())
                {
                    bot.Character.SendMessage("Experimental AI fight is NOT running");
                }
                else
                {
                    FFight fightBot = bot.GetFrame<FFight>();
                    bot.Character.SendMessage("Experimental AI fight IS running");
                    fightBot.Dump();
                }
            }
        }
    }


    internal class FFight : Frame<FFight>
    {
        private PlayedFighter _character;
        private SimplerTimer _checkTimer;
        private bool _sit = false;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private ContextActor.MoveStopHandler _stopMovingDelegate;
        private Fighter _currentTarget;
        private LOSMap _losMap;
        private BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder _pathFinder;
        public FFight(Bot bot)
            : base(bot)
        {
            bot.Character.FightJoined += OnFightJoined;
            bot.Character.FightLeft += OnFightLeft;
            bot.Character.MapJoined += OnMapJoined;
            _losMap = new LOSMap();
            if (bot.Character.IsFighting())
                OnFightJoined(bot.Character, bot.Character.Fight);
            else if (bot.Character.Map != null)
                OnMapJoined(bot.Character, bot.Character.Map);
        }

        private void OnMapJoined(PlayedCharacter character, Map map)
        {
            _pathFinder = new BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder(map, false);
            if (_checkTimer != null)
                _checkTimer.Dispose();
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

            if (Bot.Character.Stats.Health < Bot.Character.Stats.MaxHealth ) // under 90% full health
            {
                if (!_sit)
                {
                    Bot.Character.SendMessage(String.Format("Character health too low : {0}/{1} => sit", Bot.Character.Stats.Health, Bot.Character.Stats.MaxHealth));

                    Bot.CallDelayed(500, Sit);
                }

                return;
            }

            var monster = Bot.Character.Map.Actors.OfType<GroupMonster>()
                .Where(x => x.Level < Math.Min(Bot.Character.Level * 2 + 1, Bot.Character.Level + 10) && x.Level > Bot.Character.Level / 2)
                .OrderBy(x => Math.Abs(x.Level - Bot.Character.Level)).FirstOrDefault(); // As close as possible from character's level

            if (monster != null)
            {
                //Bot.Character.SendMessage(String.Format("Trying to start a fight with group lv {0}, cell {1}, leader {2} lv {3}", monster.Level, monster.Cell, monster.Leader.Name, monster.Leader.Grade.grade));
                Bot.Character.TryStartFightWith(monster, _pathFinder);
                //Move(Bot.Character.Cell, monster.Cell, false, -1);
            }
            else
                if (!Bot.Character.IsMoving())
                    Bot.Character.ChangeMap();
        }

        private void OnFightJoined(PlayedCharacter character, Fight fight)
        {
            if (_checkTimer != null)
                _checkTimer.Dispose();
            _pathFinder = new BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder(character.Map, true);

            _character = character.Fighter;
            _character.TurnStarted += OnTurnStarted;
            fight.StateChanged += OnStateChanged;
            _character.SequenceEnded += OnSequenceEnd;
            _character.Acknowledge += OnAcknowledgement;
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
            if (_character != null)
            {
                _character.SequenceEnded -= OnSequenceEnd;
                _character.Acknowledge -= OnAcknowledgement;
            }
            /*if (_stopMovingDelegate != null)
            {
                if (_character != null) _character.StopMoving -= _stopMovingDelegate;
                _stopMovingDelegate = null;
            }*/
            if (_character != null) _character.TurnStarted -= OnTurnStarted;
            fight.StateChanged -= OnStateChanged;
            _character.Dispose();
            _character = null;
            _pathFinder = new BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder(character.Map, false);
        }

        bool DoNotMoveAgain;
        private void OnTurnStarted(Fighter fighter)
        {
            _losMap.UpdateTargetCell(null, false, true, true);
            DoNotMoveAgain = false;
            Bot.CallDelayed(500, StartAI);
        }



        private void StartAI()
        {
            if (_character == null) return;
            /*if (_stopMovingDelegate != null)
            {
                _character.StopMoving -= _stopMovingDelegate;
                _stopMovingDelegate = null;
            }*/

            _currentTarget = GetNearestEnemy(); // Better nearest here than weakest. Even if the weakest should be the nearest, it's not a sure thing. 

            //Bot.Character.SendMessage(String.Format("StartAI AP:{0}, MP:{1}, Pos:{2}, Target:{3} ({4})", Bot.Character.Stats.CurrentAP, Bot.Character.Stats.CurrentMP, _character.Cell, _currentTarget, _currentTarget.Cell), Color.Pink);

            _losMap.UpdateTargetCell(_currentTarget, false, true, false);

            if (_currentTarget == null)
            {
                Bot.Character.SendMessage("No target => Pass turn", Color.Pink);
                PassTurn();
                return;
            }

            int maxDistanceWished = -1;
            uint minDistance = 1;
            bool inLine = false;
            bool inDiagonal = false;
            bool needLOSCheck = false;
            //_isLastAction = false;

            foreach (Spell spell in _character.GetOrderListOfSimpleAttackSpells(_currentTarget, true))
            {
                bool inRange = _character.IsInSpellRange(_currentTarget.Cell, spell.LevelTemplate);
                bool LoSisOK = !spell.LevelTemplate.castTestLos || _character.Fight.CanBeSeen(_character.Cell, _currentTarget.Cell, false);
                if (!inRange || !LoSisOK)
                {
                    if (ComeAtSpellRangeWithNoRisk(_currentTarget, spell.LevelTemplate.minRange, _character.GetRealSpellRange(spell.LevelTemplate), spell.LevelTemplate.castInLine, spell.LevelTemplate.castInDiagonal, spell.LevelTemplate.castTestLos))
                    {
                        Bot.Character.SendMessage(String.Format("StartAI - moving at range to cast Spell {0} (cat {2}) on {1}", spell, _currentTarget, spell.Categories), Color.Pink);
                        return;
                    }
                    else
                        if (maxDistanceWished == -1)
                        { // Keep the best one only
                            maxDistanceWished = _character.GetRealSpellRange(spell.LevelTemplate);
                            inLine = spell.LevelTemplate.castInLine;
                            inDiagonal = spell.LevelTemplate.castInDiagonal;
                            needLOSCheck = spell.LevelTemplate.castTestLos;
                            minDistance = spell.LevelTemplate.minRange;
                        }
                }
                else
                    if (!_character.CastSpell(spell, _currentTarget.Cell))
                        _character.Character.SendMessage(String.Format("For some reason, {0} can't cast the spell {1} on {2}", _character.Name, spell, _currentTarget));
                    else
                    {
                        LastActionDesc = "Spell " + spell + " casted";
                        Bot.Character.SendMessage(String.Format("StartAI casting Spell {0} (cat {2}) on {1}", spell, _currentTarget, spell.Categories), Color.Pink);

                        //_spellCastedDelegate = (sender, spellCast) => StartAI();
                        return; // EndSenquence will call StartAI again
                    }
            }

            // Can't cast an attack => try to cast some boost, heal or invoc, or just pass
            foreach (var spell in _character.GetOrderListOfSimpleBoostSpells())
            {
                if (!_character.CastSpell(spell, _character.Cell))
                    _character.Character.SendMessage(String.Format("For some reason, {0} can't cast the spell {1} on himself", _character.Name, spell));
                else
                {
                    LastActionDesc = "Spell " + spell + " casted";
                    Bot.Character.SendMessage(String.Format("StartAI casting Spell {0} (cat {2}) on himself, cell {3} (monster cell : {1})]", spell, _currentTarget.Cell, spell.Categories, _character.Cell), Color.Pink);
                    //Bot.Character.SendMessage(String.Format("StartAI casting Spell {0} on himself]", spell), Color.Pink);

                    //_spellCastedDelegate = (sender, spellCast) => StartAI();
                    return; // EndSenquence will call StartAI again
                }
            }

            if (_character.Stats.CurrentMP <= 0)
            {
                Bot.CallDelayed(250, PassTurn);
                return;
            }

            // No other spells can be cast (out of AP) => move away and pass the turn
            if (maxDistanceWished == -1)
            {
                Bot.Character.SendMessage(String.Format("StartAI No spell castable, moving away {0}pm", _character.Stats.CurrentMP), Color.Pink);
                MoveFar(false);
                return;
            }

            // If no spell is at range, then try to come closer and try again. No need to be cautious here : we can't cast anything else
            MoveNear(_currentTarget, Math.Max(1, minDistance), maxDistanceWished, inLine, inDiagonal, needLOSCheck, false);
        }

        //private bool _isLastAction;
        private void PassTurn()
        {
            //Bot.Character.SendMessage("PassTurn", Color.Pink);
            if (_character != null)
                _character.PassTurn();
        }

        string LastActionDesc;
        private void OnSequenceEnd(Fighter fighter)
        {
            //Bot.Character.SendMessage(String.Format("OnSequenceEnd AP:{0}, MP:{1}, LastAction:{2} ({3})", Bot.Character.Stats.CurrentAP, Bot.Character.Stats.CurrentMP, _isLastAction, LastActionDesc), Color.Pink);
            //LastActionDesc = "OnSequenceEnd";
            //if (!_isLastAction)
            //  Bot.CallDelayed(300, StartAI);
            //else
            //  Bot.CallDelayed(300, PassTurn);
        }

        private void OnAcknowledgement(bool failed)
        {
            Bot.Character.SendMessage(String.Format("OnAcknowledgement AP:{0}, MP:{1}, LastAction {3} failed ? {2}", Bot.Character.Stats.CurrentAP, Bot.Character.Stats.CurrentMP, failed, LastActionDesc), Color.Pink);
            LastActionDesc = "OnSequenceEnd";
            if (!failed)
                Bot.CallDelayed(300, StartAI);
            else
                Bot.CallDelayed(300, PassTurn);
        }

        /*private void OnStopMoving(MovementBehavior movement, bool Canceled, bool refused)
        {
            Bot.Character.Fighter.StopMoving -= _stopMovingDelegate;
            _stopMovingDelegate = null;

            // MP update is now handled by GameActionFightPointsVariationMessage massage
            //if (!refused)
            //    Bot.Character.Stats.CurrentMP -= movement.MovementPath.MPCost;
            //StartAI();                       
        }*/



        private void FindOptimalPlacement()
        {
            _currentTarget = FindWeakestEnemy();
            IEnumerable<Spell> spells = _character.GetOrderListOfSimpleAttackSpells(_currentTarget, true);
            Bot.Character.SendMessage(String.Format("Ordered attack spells agains {0} : {1}", _currentTarget, String.Join(",", spells)), Color.Blue);
                
            Spell bestSpell = spells.FirstOrDefault();
            if (bestSpell == null || _currentTarget == null)
            {
                Bot.Character.SendMessage(String.Format("FindOptimalPlacement : can't find a position (bestSpell: {0}, weakestEnnemy: {1})", bestSpell, _currentTarget), Color.Red);
                PlaceToWeakestEnemy();
                return;
            }
            Bot.Character.SendMessage(String.Format("FindOptimalPlacement : bestSpell: {0}, weakestEnnemy: {1} ({2}), range: {3}", bestSpell, _currentTarget, _currentTarget.Cell, _character.GetRealSpellRange(bestSpell.LevelTemplate)));

            PlaceAtDistanceFromWeakestEnemy(_character.GetRealSpellRange(bestSpell.LevelTemplate), bestSpell.LevelTemplate.castInLine);
        }

        private Fighter FindWeakestEnemy()
        {
            return _character.GetOpposedTeam().FightersAlive.OrderBy(x => x.Level).FirstOrDefault();
        }

        /// <summary>
        /// Initial placement : find the position the closest to the weakest ennemy
        /// </summary>
        private void PlaceToWeakestEnemy()
        {
            var enemy = FindWeakestEnemy();
            if (enemy == null)
            {
                Bot.Character.SendMessage("PlaceToWeakestEnemy : no enemy left ?", Color.Red);
                return;
            }

            var cell = Bot.Character.Fighter.Team.PlacementCells.OrderBy(x => x.ManhattanDistanceTo(enemy.Cell)).FirstOrDefault();
            Bot.Character.Fighter.ChangePrePlacement(cell);
        }

        /// <summary>
        /// Select the best starting position, close to the weakest monster, in proper distance for best spell, 
        /// and as far as possible from other monsters. 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="InLine"></param>
        private void PlaceAtDistanceFromWeakestEnemy(int distance, bool InLine)
        {
            // TODO : also consider LOS      

            Fighter weakestEnemy = FindWeakestEnemy();
            if (weakestEnemy == null)
            {
                logger.Warn("PlaceAtDistanceFromWeakestEnemy : weakestEnemy is null");
                return;
            }
            // find the cells under distance from weakestEnemy, and - if needed - in line
            Cell[] startingSet = Bot.Character.Fighter.Team.PlacementCells.Where(cell => ((cell.ManhattanDistanceTo(weakestEnemy.Cell) <= distance) && (!InLine || cell.X == weakestEnemy.Cell.X || cell.Y == weakestEnemy.Cell.Y))).ToArray();
            if (startingSet.Length == 0)
            {
                logger.Debug("No cell at range => PlaceToWeakestEnemy");
                PlaceToWeakestEnemy();
                return;
            }
            logger.Debug("Placement of {0} vs {1} (cell {2}) - max Distance {4} - InLine {5} - choices : {3}", _character.Name, weakestEnemy.ToString(), weakestEnemy.Cell, string.Join<Cell>(",", startingSet), distance, InLine);

            Cell[] finalSet = startingSet;
            if (finalSet.Length > 1 && _character.GetOpposedTeam().FightersAlive.Length > 1)
            {
                // remove all cells where another enemy is closer
                foreach (Fighter otherEnnemy in _character.GetOpposedTeam().FightersAlive)
                    if (otherEnnemy != weakestEnemy)
                        finalSet = finalSet.Where(x => x.ManhattanDistanceTo(otherEnnemy.Cell) >= x.ManhattanDistanceTo(weakestEnemy.Cell)).ToArray();
                logger.Debug("Rule 1 : choices {0}", string.Join<Cell>(",", finalSet));

                // if none, then we only remove cells where we are in contact of any other ennemy 
                if (startingSet.Length == 0)
                {
                    finalSet = startingSet;
                    foreach (Fighter otherEnnemy in _character.GetOpposedTeam().FightersAlive)
                        if (otherEnnemy != weakestEnemy)
                            finalSet = finalSet.Where(x => x.ManhattanDistanceTo(otherEnnemy.Cell) > 1).ToArray();
                    logger.Debug("Rule 2 : choices {0}", string.Join<Cell>(",", finalSet));
                }

                // if still none, just keep all cells, ignoring other enemies
                if (finalSet.Length == 0)
                {
                    finalSet = startingSet;
                    logger.Debug("Rule 3 (full set) : choices {0}", string.Join<Cell>(",", finalSet));
                }
            }
            // Find a cell as far as possible from weakest ennemy, but not over distance
            var bestCell = finalSet.OrderBy(x => x.ManhattanDistanceTo(weakestEnemy.Cell)).LastOrDefault();

            // If none under distance, then the closest
            if (bestCell == null)
            {
                logger.Debug("No cell at range => PlaceToWeakestEnemy");
                PlaceToWeakestEnemy();
                return;
            }
            //logger.Debug("Cell selected : {0}, distance {1}", bestCell, bestCell.ManhattanDistanceTo(weakestEnemy.Cell));
            Bot.Character.Fighter.ChangePrePlacement(bestCell);
        }

        /// <summary>
        /// If a proper path can be found, then move and return true. 
        /// Otherwise, do nothing and return false;
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="mp"></param>
        /// <param name="maxDistanceWished"></param>
        /// <param name="inLine"></param>
        /// <param name="inDiagonal"></param>
        /// <param name="needLOSCheck"></param>
        /// <returns></returns>
        private bool ComeAtSpellRangeWithNoRisk(Fighter fighter, uint minRange, int maxDistanceWished, bool inLine, bool inDiagonal, bool needLOSCheck)
        {
            Cell dest = GetCellAtSpellRange(fighter, minRange, maxDistanceWished, inLine, inDiagonal, needLOSCheck, true, true);
            if (dest == null) return false;
            Path path = ((IAdvancedPathFinder)_pathFinder).FindPath(_character.Cell, dest, false, _character.Stats.CurrentMP, 0, true);

            return !_character.Move(path);
        }


        private Cell GetCellAtSpellRange(Fighter fighter, uint minRange, int maxDistanceWished, bool inLine, bool inDiagonal, bool needLOSCheck, bool cautious, bool getOnlyExactResult)
        {
            Cell dest = null;
            IEnumerable<Cell> allMoves = GetPossibleMoves(cautious);
            Bot.Character.ResetCellsHighlight();
            Bot.Character.HighlightCells(allMoves, Color.Orange);
            IEnumerable<Cell> selectedMoves = allMoves;
            if (needLOSCheck)
            {
                _losMap.UpdateTargetCell(fighter, false, true, false);
                selectedMoves = _losMap.GetCellsSeenByTarget(selectedMoves);
            }
            IEnumerable<Cell> bestSelectedMoves = selectedMoves;
            
            // Restrict selection as needed for InLine or InDiagonal constrains
            if (inLine || inDiagonal)
                bestSelectedMoves = bestSelectedMoves.Where(cell => inLine && (fighter.Cell.X == cell.X || fighter.Cell.Y == cell.Y) || inDiagonal && (Math.Abs(fighter.Cell.X - cell.X) == Math.Abs(fighter.Cell.Y - cell.Y)));
            
            // Keep only those cells that respect max distance to target
            bestSelectedMoves = bestSelectedMoves.Where(cell => cell.ManhattanDistanceTo(fighter.Cell) <= maxDistanceWished && cell.ManhattanDistanceTo(fighter.Cell) >= minRange);

            // As the cells are initialy sorted by distance from the starting position, just get the first one if any. 
            dest = bestSelectedMoves.FirstOrDefault();
            if (getOnlyExactResult || dest != null) return dest;

            // If no result and no need to have a proper result, come as close as possible to the target
            
            // Find the closest position that is equal or under maxDistanceWished of the target, if any
            dest = allMoves.FirstOrDefault(cell => cell.ManhattanDistanceTo(fighter.Cell) <= maxDistanceWished);
            if (dest != null) return dest;
            
            // as we are far from the target, use PathFinding to find the shortest path to reach the target
            Path path = ((IAdvancedPathFinder)_pathFinder).FindPath(_character.Cell, fighter.Cell, false, _character.Stats.CurrentMP);
            return path.End;
        }


        private void MoveNear(Fighter fighter, uint minRange, int maxDistanceWished, bool inLine, bool inDiagonal, bool needLOSCheck, bool cautious)
        {
            //Bot.Character.SendMessage(String.Format("MoveNear {0} : mp = {1}, distanceWished = {2}, inLine = {3}, inDiagonal = {4}, LOS = {5}", fighter, mp, maxDistanceWished, inLine, inDiagonal, needLOSCheck), Color.Pink);
            //_stopMovingDelegate = (sender, behavior, canceled, refused) => OnStopMoving(behavior, canceled, refused);
            //Bot.Character.Fighter.StopMoving += _stopMovingDelegate;

            Cell dest = GetCellAtSpellRange(fighter, minRange, maxDistanceWished, inLine, inDiagonal, needLOSCheck, cautious, false);

            if (dest == null)
            {
                Bot.Character.SendMessage(String.Format("Can't Move near {0}", fighter), Color.Red);
                PassTurn();
                return;
            }
            //Bot.Character.SendMessage(res, Color.Pink);

            //Move(_character.Cell, dest, true, mp);
            if (!_character.Move(dest, _pathFinder))
            {
                Bot.Character.SendMessage(String.Format("Failed to move from {0} to {1}, near {2} => pass", _character.Cell, dest, fighter), Color.Red);
                PassTurn();
            }
            else
                LastActionDesc = "MoveNear";
        }

        /// <summary>
        /// Gives walkable cells where the character may walk within the turn.
        /// Note : it's now supposed to be reliable, using PathFinder
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Cell> GetPossibleMoves(bool cautious)
        {
            return _pathFinder.FindConnectedCells(
                _character.Cell, true, cautious,
                cell => cell.DistanceSteps <= _character.Stats.CurrentMP);
            
            /*Lozenge shape = new Lozenge(0, (byte)_character.Stats.CurrentMP);

            // It's quite inefficient, but well, no real need to optimize here
            return shape.GetCells(_character.Cell, _character.Map).Where(cell => _character.Fight.IsCellWalkable(cell, false, _character.Cell)).ToArray();*/
        }

        private void MoveFar(bool cautious)
        {
            if (DoNotMoveAgain) return;
            //_isLastAction = true;
            IEnumerable<Cell> enemyCells = _character.GetOpposedTeam().FightersAlive.Select(fighter => fighter.Cell);
            
            /*Cell dest = GetPossibleMoves().Where(cell => _character.Fight.IsCellWalkable(cell, false, _character.Cell)).OrderByDescending(cell => enemies.Min(ennCell => cell.ManhattanDistanceTo(ennCell.Cell))).FirstOrDefault();
            // Do not move if the new position is not better than the old one. 
            if (enemies.Min(ennCell => dest.ManhattanDistanceTo(ennCell.Cell)) < enemies.Min(ennCell => _character.Cell.ManhattanDistanceTo(ennCell.Cell)))
            {                
                DoNotMoveAgain = true;
                return;
            }*/
            uint ActualDistanceFromEnnemies = enemyCells.Min(ennCell => _character.Cell.ManhattanDistanceTo(ennCell));
            DoNotMoveAgain = true;
            if (_character.Stats.CurrentMP < 1) return;
            Cell dest = _pathFinder.FindConnectedCells(
                _character.Cell, true, cautious,
                cell => cell.DistanceSteps <= _character.Stats.CurrentMP && enemyCells.Min(ennCell => cell.Cell.ManhattanDistanceTo(ennCell)) > ActualDistanceFromEnnemies,
                cell => enemyCells.Min(ennCell => (int)cell.Cell.ManhattanDistanceTo(ennCell))).FirstOrDefault();

            Debug.Assert((enemyCells.Min(ennCell => dest.ManhattanDistanceTo(ennCell)) > ActualDistanceFromEnnemies), "This move do not take the character away from monsters !");
            if (dest == null)
            {
                DoNotMoveAgain = true;
                PassTurn();
                return;
            }

            //Move(_character.Cell, dest, true, _character.Stats.CurrentMP);
            if (!_character.Move(dest, _pathFinder))
            {
                Bot.Character.SendMessage(String.Format("Failed to move far from {0} to {1}, away from {2} => pass", _character.Cell, dest, String.Join(",", enemyCells)), Color.Red);
                PassTurn();
            }
            else
            {
                Bot.Character.SendMessage(String.Format("Moving away from ({0}) : pos {1}({2}) => {3}({4}) => pass", String.Join(",", enemyCells), _character.Cell, ActualDistanceFromEnnemies, dest, enemyCells.Min(ennCell => dest.ManhattanDistanceTo(ennCell))), Color.Pink);
                
                LastActionDesc = "MoveFar";
            }
        }

        private Fighter GetNearestEnemy()
        {
            if (_character == null) return null;
            var enemyTeam = _character.GetOpposedTeam();
            return enemyTeam.FightersAlive.OrderBy(enemy => _character.Cell.ManhattanDistanceTo(enemy.Cell)).FirstOrDefault();
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
                Bot.Character.StartMoving -= StandUp;
            }

            if (_character != null)
            {
                _character.TurnStarted -= OnTurnStarted;
                if (_character.Fight != null)
                    _character.Fight.StateChanged -= OnStateChanged;
                _character.SequenceEnded -= OnSequenceEnd;
                _character.Acknowledge -= OnAcknowledgement;
                _character = null;
            }

            if (_checkTimer != null)
                _checkTimer.Dispose();

            base.OnDetached();
        }

        internal void Dump()
        {
            Bot.Character.SendMessage(String.Format("Current target : {0}", _currentTarget));
            Bot.Character.SendMessage(String.Format("HP : {0}, AP : {1}, MP : {2}", Bot.Character.Stats.Health, Bot.Character.Stats.CurrentAP, Bot.Character.Stats.CurrentMP));
            Bot.Character.SendMessage(String.Format("Sitting : {0}, Moving : {1}, Fighting : {2}", _sit, _character != null ? _character.IsMoving() : Bot.Character.IsMoving(), Bot.Character.IsFighting()));
            Bot.Character.SendMessage(String.Format("MapID : {0}, Cell : {1}", _character != null ? _character.Map.Id : Bot.Character.Map.Id, _character != null ? _character.Cell : Bot.Character.Cell));
        }
    }
}