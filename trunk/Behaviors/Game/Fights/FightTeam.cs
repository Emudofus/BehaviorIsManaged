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

        public void AddFighter(Fighter fighter)
        {
            if (Fighters.Any(x => x.Id == fighter.Id))
                throw new Exception(string.Format("Fighter with id {0} already exists", fighter.Id));

            m_fighters.Add(fighter);

            var evnt = FighterAdded;
            if (evnt != null)
                evnt(this, fighter);
        }

        public bool RemoveFighter(int id)
        {
            var result = m_fighters.RemoveAll(entry => entry.Id == id);

            if (result > 1)
                logger.Error("Removed {0} fighters instead of one.", result);

            return result > 0;
        }

        public bool RemoveFighter(Fighter fighter)
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
                logger.Error("Cannot set the leader because fighter {0} is not in team", team.leaderId);
            }
            AlignmentSide = (AlignmentSideEnum) team.teamSide;
            TeamType = (TeamTypeEnum) team.teamTypeId;
            
            // don't care about the figthers infos
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}