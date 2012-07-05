using System;
using BiM.Behaviors.Game.World.Pathfinding;

namespace BiM.Behaviors.Game.Actors.RolePlay
{

    public abstract class RolePlayActor : ContextActor
    {
        public delegate void MoveStartHandler(RolePlayActor actor, Path path);
        public event MoveStartHandler StartMoving;

        public virtual void NotifyStartMoving(Path path)
        {
            MovePath = path;
            MoveStartTime = DateTime.Now;

            if (MovePath.Start != Position)
            {
                // todo : log
                Position = MovePath.Start;
            }

            var handler = StartMoving;
            if (handler != null) handler(this, path);
        }

        public delegate void MoveStopHandler(RolePlayActor actor);
        public event MoveStartHandler StopMoving;

        public virtual void NotifyStopMoving()
        {
            if (MovePath == null)
                throw new InvalidOperationException("Entity was not moving");

            Position = MovePath.End;

            var handler = StopMoving;
            if (handler != null) handler(this, MovePath);

            MovePath = null;
            MoveStartTime = null;
        }


        public override int Id
        {
            get;
            protected set;
        }

        public DateTime? MoveStartTime
        {
            get;
            protected set;
        }

        public Path MovePath
        {
            get;
            protected set;
        }

        public bool IsMoving()
        {
            return MovePath != null;
        }

        public override void Dispose()
        {
            StartMoving = null;
            StopMoving = null;

            base.Dispose();
        }
    }
}