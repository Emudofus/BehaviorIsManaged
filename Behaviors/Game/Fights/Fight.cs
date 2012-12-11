#region License GNU GPL
// Fight.cs
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
using System.Collections.Generic;
using System.Linq;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;
using System.Drawing;

namespace BiM.Behaviors.Game.Fights
{
    public class Fight : MapContext<Fighter>
    {

        public delegate void StateChangedHandler(Fight fight, FightPhase phase);
        public event StateChangedHandler StateChanged;

        protected void OnStateChanged(FightPhase phase)
        {
            StateChangedHandler handler = StateChanged;
            if (handler != null) handler(this, phase);
        }


        public delegate void TurnHandler(Fight fight, Fighter fighter);
        public event TurnHandler TurnStarted;
        public event TurnHandler TurnEnded;
        public event TurnHandler SequenceEnded;

        public delegate void ActorAddedRemovedHandler(Fight fight, Fighter fighter);
        public event ActorAddedRemovedHandler ActorAdded;

        public void OnActorAdded(Fighter fighter)
        {
            ActorAddedRemovedHandler handler = ActorAdded;
            if (handler != null) handler(this, fighter);
        }

        public event ActorAddedRemovedHandler ActorRemoved;

        public void OnActorRemoved(Fighter fighter)
        {
            ActorAddedRemovedHandler handler = ActorRemoved;
            if (handler != null) handler(this, fighter);
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public Fight(GameFightJoinMessage msg, Map map)
        {
            Map = map;
            StartTime = DateTime.Now - TimeSpan.FromMilliseconds(msg.timeMaxBeforeFightStart);
            CanCancelFight = msg.canBeCancelled;
            CanSayReady = msg.canSayReady;
            IsSpectator = msg.isSpectator;
            Type = (FightTypeEnum) msg.fightType;

            RedTeam = new FightTeam(this, FightTeamColor.Red);
            BlueTeam = new FightTeam(this, FightTeamColor.Blue);

            if (msg.timeMaxBeforeFightStart > 0)
                Phase = FightPhase.Placement;

            TimeLine = new TimeLine(this);
        }

        public int Id
        {
            get;
            private set;
        }

        public override CellList Cells
        {
            get { return Map.Cells; }
        }
        public override bool UsingNewMovementSystem
        {
            get
            {
                return Map.UsingNewMovementSystem;
            }
        }

        public DateTime StartTime
        {
            get;
            private set;
        }

        public TimeSpan TimeUntilStart
        {
            get { return Phase == FightPhase.Placement && StartTime > DateTime.Now ? StartTime - DateTime.Now : TimeSpan.Zero; }
        }

        public bool CanCancelFight
        {
            get;
            private set;
        }

        public bool CanSayReady
        {
            get;
            private set;
        }

        public bool IsSpectator
        {
            get;
            private set;
        }

        public FightTypeEnum Type
        {
            get;
            private set;
        }

        public FightPhase Phase
        {
            get;
            private set;
        }

        public Map Map
        {
            get;
            private set;
        }

        public FightTeam RedTeam
        {
            get;
            private set;
        }

        public FightTeam BlueTeam
        {
            get;
            private set;
        }

        public TimeLine TimeLine
        {
            get;
            private set;
        }

        public Fighter CurrentPlayer
        {
            get { return TimeLine.CurrentPlayer; }
        }

        public int Round
        {
            get;
            private set;
        }


        public override void Tick(int dt)
        {
            if (RedTeam != null)
                foreach (var fighter in RedTeam.Fighters)
                {
                    fighter.Tick(dt);
                }
            if (BlueTeam != null)
                foreach (var fighter in BlueTeam.Fighters)
                {
                    fighter.Tick(dt);
                }
        }

        public FightTeam GetTeam(sbyte color)
        {
            return GetTeam((FightTeamColor) color);
        }

        public FightTeam GetTeam(FightTeamColor color)
        {
            if (color == FightTeamColor.Red) return RedTeam;
            else if (color == FightTeamColor.Blue) return BlueTeam;
            else
                throw new Exception(string.Format("Color {0} is not a valid team id !", color));
        }

        protected void SetPhase(FightPhase phase)
        {
            Phase = phase;
            OnStateChanged(phase);
        }

        public void StartFight()
        {
            if (Phase != FightPhase.Placement)
            {
                logger.Error("Cannot start the fight : the fight is not in Placement phase (Phase={0})", Phase);
                return;
            }

            SetPhase(FightPhase.Fighting);
            StartTime = DateTime.Now;
        }

        public void EndFight(GameFightEndMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            SetPhase(FightPhase.Ended);

            // todo : manage the panel
        }

        public void SetRound(int round)
        {
            Round = round;

            //evnt
        }

        public void StartTurn(int playerId)
        {
            var fighter = GetFighter(playerId);
            
            if (fighter == null)
                throw new InvalidOperationException(string.Format("Fighter {0} not found, cannot start turn", playerId));

            TimeLine.SetCurrentPlayer(fighter);

            var evnt = TurnStarted;
            if (evnt != null)
                evnt(this, TimeLine.CurrentPlayer);

            if (TimeLine.CurrentPlayer != null)
                TimeLine.CurrentPlayer.NotifyTurnStarted();
        }

        public void EndTurn()
        {
            var evnt = TurnEnded;
            if (evnt != null)
                evnt(this, TimeLine.CurrentPlayer);

            if (TimeLine.CurrentPlayer != null)
                TimeLine.CurrentPlayer.NotifyTurnEnded();

            TimeLine.ResetCurrentPlayer();
        }

        public bool HasFightStarted()
        {
            return DateTime.Now >= StartTime;
        }

        public override bool AddActor(Fighter actor)
        {
            if (actor == null) throw new ArgumentNullException("actor");

            // do it before because an existing fighter can be removed
            bool added = base.AddActor(actor);

            actor.Team.AddFighter(actor);

            if (Phase == FightPhase.Placement)
                TimeLine.RefreshTimeLine();

            OnActorAdded(actor);

            return added;
        }

        public void AddActor(GameFightFighterInformations fighter)
        {
            if (fighter == null) throw new ArgumentNullException("fighter");

            Bot bot = BotManager.Instance.GetCurrentBot();

            var existingFighter = GetFighter(fighter.contextualId);

            if (existingFighter != null)
            {
                existingFighter.Update(fighter);
            }
            else
            {
                // normally we don't know which is our team before being added to the fight
                if (fighter.contextualId == bot.Character.Id)
                {
                    bot.Character.Fighter.SetTeam(GetTeam(fighter.teamId));
                    AddActor(bot.Character.Fighter);
                }
                else
                {
                    AddActor(CreateFighter(fighter));
                }
            }
        }

        public override bool RemoveActor(Fighter actor)
        {
            if (actor.Team.RemoveFighter(actor) && base.RemoveActor(actor))
            {
                OnActorRemoved(actor);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create a Fighter instance corresponding to the network given type
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        protected Fighter CreateFighter(GameFightFighterInformations fighter)
        {
            if (fighter == null) throw new ArgumentNullException("fighter");

            if (fighter is GameFightCharacterInformations)
                return new CharacterFighter(fighter as GameFightCharacterInformations, this);
            if (fighter is GameFightMonsterInformations)
            {
                return new MonsterFighter(fighter as GameFightMonsterInformations, this);
            }

            throw new Exception(string.Format("Fighter of type {0} not handled", fighter.GetType()));
        }


        /// <summary>
        /// To get the fighters sort by Initiative use TimeLine.Fighters
        /// </summary>
        /// <returns></returns>
        public Fighter[] GetFighters()
        {
            return Actors.ToArray();
        }

        public Fighter GetFighter(int id)
        {
            return Actors.FirstOrDefault(entry => entry.Id == id);
        }

        public T GetFighter<T>(int id) where T : Fighter
        {
            return (T) Actors.FirstOrDefault(entry => entry is T && entry.Id == id);
        }

        public Fighter GetFighter(Cell cell)
        {
            // i assume 2 fighters can be on the same cell (i.g if someone carry someone)
            return Actors.FirstOrDefault(entry => entry.Cell == cell);
        }

        public Fighter[] GetFighters(Cell centerCell, int radius)
        {
            return Actors.Where(entry => entry.Cell.IsInRadius(centerCell, radius)).ToArray();
        }

        public Fighter[] GetFighters(Cell centerCell, int minRadius, int radius)
        {
            return Actors.Where(entry => entry.Cell.IsInRadius(centerCell, minRadius, radius)).ToArray();
        }

        public void Update(GameFightPlacementPossiblePositionsMessage msg)
        {
            if (msg == null)
                throw new ArgumentException("msg");

            RedTeam.Update(msg);
            BlueTeam.Update(msg);

            SetPhase(FightPhase.Placement);
        }

        public void Update(GameEntitiesDispositionMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            foreach (IdentifiedEntityDispositionInformations disposition in msg.dispositions)
            {
                Fighter fighter = GetFighter(disposition.id);

                // seems like the client don't cares
                if (fighter != null)
                    fighter.Update(disposition);
            }
        }

        public void Update(GameFightUpdateTeamMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Id = msg.fightId;

            GetTeam(msg.team.teamId).Update(msg.team);
        }

        public void Update(GameFightHumanReadyStateMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Fighter fighter = GetFighter(msg.characterId);

            fighter.IsReady = msg.isReady;
        }

        public void Update(Bot bot, GameFightSynchronizeMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            
            TimeLine.Update(msg);


            if (bot.Character.Fighter != null)
                bot.Character.Fighter.Update(msg);
            foreach (var info in msg.fighters)
            {
                var fighter = GetFighter(info.contextualId);

                if (fighter == null)
                {
                    logger.Error("(GameFightSynchronizeMessage) Fighter {0} not found", info.contextualId);
                }
                else
                {
                    fighter.Update(info);
                }
            }
        }

        public void Update(GameFightRefreshFighterMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            if (!( msg.informations is GameFightFighterInformations ))
            {
                logger.Error("(GameFightRefreshFighterMessage) Cannot update a fighter with a {0} instance", msg.informations.GetType());
            }
            else
            {
                var fighter = GetFighter(msg.informations.contextualId);

                if (fighter == null)
                {
                    logger.Error("(GameFightRefreshFighterMessage) Fighter {0} not found", msg.informations.contextualId);
                }
                else
                {
                    fighter.Update(msg.informations as GameFightFighterInformations);
                }
            }

            if (Phase == FightPhase.Placement)
                TimeLine.RefreshTimeLine();
        }

        public void Update(GameFightTurnListMessage msg)
        {            
            if (msg == null) throw new ArgumentNullException("msg");
            foreach (var deadsId in msg.deadsIds)
            {
                var fighter = GetFighter(deadsId);

                if (fighter == null)
                {
                    logger.Error("(GameFightTurnListMessage) Fighter {0} not found", deadsId);
                }
                else
                {
                    fighter.IsAlive = false;
                }
            }

            foreach (var id in msg.ids)
            {
                var fighter = GetFighter(id);

                if (fighter == null)
                {
                    logger.Error("(GameFightTurnListMessage) Fighter {0} not found", id);
                }
                else
                {
                    fighter.IsAlive = true;
                }
            }

            TimeLine.RefreshTimeLine(msg.ids);
        }

        public void Update(GameFightOptionStateUpdateMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Id = msg.fightId;
            GetTeam(msg.teamId).Update(msg);
        }

        public void EndSequence(SequenceEndMessage message)
        {
            var fighter = GetFighter(message.authorId);
            
            if (fighter == null)
                throw new InvalidOperationException(string.Format("Fighter {0} not found, cannot end sequence", message.authorId));

            if (SequenceEnded != null)
                SequenceEnded(this, fighter);

            //if ((TimeLine.CurrentPlayer != null) && (message.authorId != TimeLine.CurrentPlayer.Id))
            //    throw new InvalidOperationException(string.Format("EndSequence authorId {0} is not current Player {1}", message.authorId, fighter.Id));
            //if (CurrentPlayer is PlayedFighter)
            //{
            //    (CurrentPlayer as PlayedFighter).Character.SendMessage(String.Format("EndSequence : of fighter {0}, CurrentPlayer {1}, TimeLine.CurrentPlayer {2}", fighter, CurrentPlayer, TimeLine.CurrentPlayer), Color.Gray);
            //}
            if (CurrentPlayer != null && CurrentPlayer.Id == message.authorId)
                CurrentPlayer.NotifySequenceEnded();
        }

        internal void Update(Bot bot, GameActionFightDeathMessage message)
        {
              // Process
            var Fighter = GetActor(message.targetId);
            if (Fighter == null)
                throw new InvalidOperationException(string.Format("Fighter {0} not found, cannot let it die", message.targetId));
            Fighter.IsAlive = false;
            bot.Character.Fighter.SummonUpdate(message);            
        }
    }
}