using System;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public abstract class Fighter : ContextActor
    {
        public delegate void TurnHandler(Fighter fighter);
        public event TurnHandler TurnStarted;

        public void NotifyTurnStarted()
        {
            TurnHandler handler = TurnStarted;
            if (handler != null) handler( this);
        }

        public event TurnHandler TurnEnded;

        public void NotifyTurnEnded()
        {
            TurnHandler handler = TurnEnded;
            if (handler != null) handler(this);
        }

        private bool m_isReady;

        public Fight Fight
        {
            get;
            protected set;
        }

        public FightTeam Team
        {
            get;
            protected set;
        }

        public virtual bool IsAlive
        {
            get;
            set;
        }

        public bool IsReady
        {
            get { return m_isReady; }
            set
            {
                m_isReady = value;
                Action<Fighter, bool> evnt = ReadyStateChanged;

                if (evnt != null)
                    evnt(this, value);
            }
        }

        public virtual string Name
        {
            get;
            protected set;
        }

        public virtual int Level
        {
            get;
            protected set;
        }

        public virtual MinimalStats Stats
        {
            get;
            protected set;
        }

        public override IContext Context
        {
            get { return Fight; }
            protected set
            {
                // not used
                Fight = (Fight) value;
            }
        }

        /// <summary>
        /// Returns true whenever it's fighter's turn
        /// </summary>
        /// <returns></returns>
        public virtual bool IsPlaying()
        {
            return this == Fight.TimeLine.CurrentPlayer;
        }

        /// <summary>
        /// Returns true whenever the fighter is able to play (i.g not dead)
        /// </summary>
        /// <returns></returns>
        public virtual bool CanPlay()
        {
            return IsAlive;
        }

        public event Action<Fighter, bool> ReadyStateChanged;

        public void Update(EntityDispositionInformations disposition)
        {
            if (disposition == null) throw new ArgumentNullException("disposition");
            Position.Cell = Map.Cells[disposition.cellId];
            Position.Direction = (DirectionsEnum) disposition.direction;
        }

        public virtual void Update(GameFightFighterInformations informations)
        {
            IsAlive = informations.alive;
            Stats.Update(informations.stats);
            Update(informations.disposition);
        }
    }
}