using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Fights
{
    public class TimeLine
    {
        private List<Fighter> m_fighters = new List<Fighter>();
        private ReadOnlyObservableCollection<Fighter> m_readOnlyFighters;

        public TimeLine(Fight fight)
        {
            Fight = fight;
            Index = -1;
            m_readOnlyFighters = new ReadOnlyObservableCollection<Fighter>(new ObservableCollection<Fighter>(m_fighters));
        }

        public Fight Fight
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollection<Fighter> Fighters
        {
            get
            {
                return m_readOnlyFighters;
            }
        }

        public int Index
        {
            get;
            private set;
        }

        public Fighter CurrentPlayer
        {
            get;
            private set;
        }

        public int GetFighterIndex(Fighter fighter)
        {
            return Fighters.IndexOf(fighter);
        }

        public void ResetCurrentPlayer()
        {
            CurrentPlayer = null;
            Index = -1;
        }

        public void SetCurrentPlayer(Fighter fighter)
        {
            CurrentPlayer = fighter;

            var index = GetFighterIndex(fighter);

            if (index == -1)
                throw new Exception(string.Format("Something goes wrong, fighter {0} not found in the timeline", fighter.Id));

            Index = index;
        }

        /// <summary>
        /// Get all fighters who will play before the turn of <paramref name="fighter"/>
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        public Fighter[] GetNextPlayers(Fighter fighter)
        {
            var result = new List<Fighter>();
            if (CurrentPlayer == fighter || Fighters.Count == 0 || Fighters.All(entry => !entry.CanPlay()))
            {
                return new Fighter[0];
            }

            var currentFighter = GetNextPlayer();
            int index = ( Index + 1 ) < Fighters.Count ? Index + 1 : 0;
            while (currentFighter != fighter)
            {
                if (currentFighter.CanPlay())
                    result.Add(currentFighter);

                index = ( index + 1 ) < Fighters.Count ? index + 1 : 0;
            }

            return result.ToArray();
        }

        public Fighter GetNextPlayer()
        {
            if (Fighters.Count == 0 || Fighters.All(entry => !entry.CanPlay()))
            {
                return null;
            }

            int counter = 0;
            int index = ( Index + 1 ) < Fighters.Count ? Index + 1 : 0;
            while (!Fighters[index].CanPlay() && counter < Fighters.Count)
            {
                index = ( index + 1 ) < Fighters.Count ? index + 1 : 0;

                counter++;
            }

            if (!Fighters[index].CanPlay()) // no fighter can play
            {
                Index = -1;
                return null;
            }

            return Fighters[index];
        }

        public void Update(GameFightSynchronizeMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            var timeLine = msg.fighters.Select(entry => Fight.GetFighter(entry.contextualId));

            m_fighters.Clear();
            m_fighters.AddRange(timeLine);
        }
    }
}