using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Stats;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Fights
{
    public class TimeLine
    {
        private ObservableCollection<Fighter> m_fighters = new ObservableCollection<Fighter>();
        private ReadOnlyObservableCollection<Fighter> m_readOnlyFighters;

        public TimeLine(Fight fight)
        {
            Fight = fight;
            Index = -1;
            m_readOnlyFighters = new ReadOnlyObservableCollection<Fighter>(m_fighters);
        }

        public Fight Fight
        {
            get;
            private set;
        }

        /// <summary>
        /// This list is ordered by the fighters turns
        /// </summary>
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

        private int GetRealInitiative(Fighter fighter)
        {
            return fighter.Stats.Initiative * fighter.Stats.Health / fighter.Stats.MaxHealth;
        }

        public void RefreshTimeLine()
        {
            IOrderedEnumerable<Fighter> redFighters = Fight.RedTeam.Fighters.
              OrderByDescending(GetRealInitiative);
            IOrderedEnumerable<Fighter> blueFighters = Fight.BlueTeam.Fighters.
                OrderByDescending(GetRealInitiative);

            bool redFighterFirst = !(Fight.RedTeam.Fighters.Count == 0 || Fight.BlueTeam.Fighters.Count == 0) &&
                GetRealInitiative(redFighters.First()) > GetRealInitiative(blueFighters.First());

            IEnumerator<Fighter> redEnumerator = redFighters.GetEnumerator();
            IEnumerator<Fighter> blueEnumerator = blueFighters.GetEnumerator();
            var timeLine = new List<Fighter>();

            bool hasRed;
            bool hasBlue = false;
            while (( hasRed = redEnumerator.MoveNext() ) | ( hasBlue = blueEnumerator.MoveNext() ))
            {
                if (redFighterFirst)
                {
                    if (hasRed)
                        timeLine.Add(redEnumerator.Current);

                    if (hasBlue)
                        timeLine.Add(blueEnumerator.Current);
                }
                else
                {
                    if (hasBlue)
                        timeLine.Add(blueEnumerator.Current);

                    if (hasRed)
                        timeLine.Add(redEnumerator.Current);
                }
            }

            m_fighters.Clear();
            foreach (var fighter in timeLine)
            {
                m_fighters.Add(fighter);
            }
        }

        public void RefreshTimeLine(IEnumerable<int> ids)
        {
            var timeLine = ids.Select(entry => Fight.GetFighter(entry));

            m_fighters.Clear();
            foreach (var fighter in timeLine)
            {
                m_fighters.Add(fighter);
            }
        }

        public void Update(GameFightSynchronizeMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            RefreshTimeLine(msg.fighters.Select(x => x.contextualId));
        }
    }
}