using System;
using BiM.Behaviors.Game.Fights;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public abstract class Fighter : ContextActor
    {
        public Fight Fight
        {
            get;
            set;
        }

        public FightTeam Team
        {
            get;
            set;
        }

        public virtual bool IsAlive
        {
            get;
            set;
        }

        public bool IsReady
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual int Level
        {
            get;
            set;
        }

        public override World.IContext Context
        {
            get
            {
                return Fight;
            }
            protected set
            {
                // not used
                Fight = (Fight)value;
            }
        }

        public void Update(EntityDispositionInformations disposition)
        {
            if (disposition == null) throw new ArgumentNullException("disposition");
            Position.Cell = Map.Cells[disposition.cellId];
            Position.Direction = (DirectionsEnum) disposition.direction;
        }
    }
}