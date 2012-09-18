using System;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Enums;
using NLog;

namespace BiM.Behaviors.Game.Actors.RolePlay
{

    public abstract class RolePlayActor : ContextActor
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override int Id
        {
            get;
            protected set;
        }


        public override IContext Context
        {
            get
            {
                return Map;
            }
            protected set
            {
                
            }
        }

        public Map Map
        {
            get;
            set;
        }

       
        public override void Dispose()
        {

            base.Dispose();
        }
    }
}