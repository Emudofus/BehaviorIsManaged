#region License GNU GPL
// FightTeam.cs
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Fights
{
    public class FightTeam : INotifyPropertyChanged
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public delegate void FighterAddedOrRemovedHandler(FightTeam team, Fighter fighter);

        public event FighterAddedOrRemovedHandler FighterAdded;
        public event FighterAddedOrRemovedHandler FighterRemoved;

        private List<Fighter> m_fighters = new List<Fighter>();
        private Cell[] m_placementCells;
        private int? m_unknownLeaderId;

        public FightTeam(Fight fight, FightTeamColor id)
        {
           Fight = fight;
           Id = id;
        }

        public FightTeamColor Id
        {
            get;
            private set;
        }

        public Fight Fight
        {
            get;
            private set;
        }

        public Cell[] PlacementCells
        {
            get
            {
                return m_placementCells;
            }
        }
        
        public ReadOnlyCollection<Fighter> Fighters
        {
            get { return m_fighters.AsReadOnly(); }
        }

        /// <summary>
        /// Retreives all fighters that are alive and visible
        /// </summary>
        public Fighter[] FightersAlive
        {
            get { return m_fighters.Where(x => x.IsAlive && x.Cell != null).ToArray(); }
        }

        public Fighter Leader
        {
            get;
            private set;
        }

        public AlignmentSideEnum AlignmentSide
        {
            get;
            private set;
        }

        public TeamTypeEnum TeamType
        {
            get;
            private set;
        }

        public bool IsSecret
        {
            get;
            private set;
        }

        public bool IsClosed
        {
            get;
            private set;
        }

        public bool IsRestrictedToParty
        {
            get;
            private set;
        }

        public bool IsHelpRequested
        {
            get;
            private set;
        }

        /// <summary>
        /// Used by Fight class only
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        internal void AddFighter(Fighter fighter)
        {
            if (Fighters.Any(x => x.Id == fighter.Id))
                throw new Exception(string.Format("Fighter with id {0} already exists", fighter.Id));

            m_fighters.Add(fighter);

            if (m_unknownLeaderId != null && fighter.Id == m_unknownLeaderId)
            {
                Leader = fighter;
                m_unknownLeaderId = null;
            }

            var evnt = FighterAdded;
            if (evnt != null)
                evnt(this, fighter);
        }

        /// <summary>
        /// Used by Fight class only
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        internal bool RemoveFighter(Fighter fighter)
        {
            var result = m_fighters.Remove(fighter);

            if (result)
            {
                var evnt = FighterRemoved;
                if (evnt != null)
                    evnt(this, fighter);
            }

            return result;
        }

        public Fighter GetFighter(int id)
        {
            return m_fighters.FirstOrDefault(entry => entry.Id == id);
        }

        public T GetFighter<T>(int id) where T : Fighter
        {
            return (T)m_fighters.FirstOrDefault(entry => entry is T && entry.Id == id);
        }

        public void Update(GameFightPlacementPossiblePositionsMessage msg)
        {
            if (msg == null)
                throw new ArgumentException("msg");

            m_placementCells = ( Id == FightTeamColor.Red ? msg.positionsForChallengers : msg.positionsForDefenders ).
                Select(entry => Fight.Map.Cells[entry]).ToArray();
        }

        public void Update(FightTeamInformations team)
        {
            if (team == null) throw new ArgumentNullException("team");
            if (team.teamId != (int)Id)
            {
                logger.Error("Try to update team {0} but the given object is for team {1}", Id, (FightTeamColor)team.teamId);
            }

            Leader = GetFighter(team.leaderId);
            if (Leader == null)
            {
                // if not found we define it later
                m_unknownLeaderId = team.leaderId;
            }
            AlignmentSide = (AlignmentSideEnum) team.teamSide;
            TeamType = (TeamTypeEnum) team.teamTypeId;
            
            // don't care about the figthers infos
        }

        public void Update(GameFightOptionStateUpdateMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            if (msg.fightId != Fight.Id)
            {
                logger.Warn("(GameFightOptionStateUpdateMessage) Incorrect fightid {0} instead of {1}", msg.fightId, Id);
                return;
            }

            if (msg.teamId != (int)Id)
            {
                logger.Warn("(GameFightOptionStateUpdateMessage) Incorrect teamid {0} instead of {1}", msg.fightId, Id);
                return;
            }

          
            switch ((FightOptionsEnum)msg.option)
            {
                case FightOptionsEnum.FIGHT_OPTION_SET_SECRET:
                    IsSecret = msg.state;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP:
                    IsHelpRequested = msg.state;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_SET_CLOSED:
                    IsClosed = msg.state;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY:
                    IsRestrictedToParty = msg.state;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}