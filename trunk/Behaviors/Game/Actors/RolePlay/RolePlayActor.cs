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

        public delegate void MoveStartHandler(RolePlayActor actor, MovementBehavior movement);
        public event MoveStartHandler StartMoving;

        public virtual void NotifyStartMoving(Path path)
        {
            if (path.IsEmpty())
            {
                logger.Warn("Try to start moving with an empty path");
                return;
            }

            Movement = new MovementBehavior(path, GetAdaptedVelocity(path));
            Movement.Start();

            NotifyStartMoving(Movement);
        }

        public virtual void NotifyStartMoving(MovementBehavior movement)
        {
            Movement = movement;

            if (Movement.StartCell != Position.Cell)
            {
                logger.Warn("Actor start cell incorrect for this moving path Position={0}, StartPath={1}", Position.Cell, Movement.StartCell);
                Position.Cell = Movement.StartCell;
            }

            var handler = StartMoving;
            if (handler != null) handler(this, Movement);
        }

        public delegate void MoveStopHandler(RolePlayActor actor, MovementBehavior movement, bool canceled);
        public event MoveStopHandler StopMoving;

        public virtual void NotifyStopMoving(bool canceled)
        {
            if (Movement == null)
                throw new InvalidOperationException("Entity was not moving");

            if (canceled)
            {
                Movement.Cancel();

                var element = Movement.TimedPath.GetCurrentElement();
                Position = new ObjectPosition(Map, element.CurrentCell, element.Direction);
            }
            else
                Position = MovementPath.EndPosition.Clone();

            var movement = Movement;
            Movement = null;

            var handler = StopMoving;
            if (handler != null) handler(this, movement, canceled);
        }

        public virtual VelocityConfiguration GetAdaptedVelocity(Path path)
        {
            return MovementBehavior.WalkingMovementBehavior;
        }

        public override int Id
        {
            get;
            protected set;
        }

        public MovementBehavior Movement
        {
            get;
            protected set;
        }

        public DateTime? MovementStartTime
        {
            get
            {
                return Movement.StartTime;
            }
        }

        public Path MovementPath
        {
            get { return Movement.MovementPath; }
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

        public override void Tick(int dt)
        {
            base.Tick(dt);

            if (Movement != null && Movement.IsEnded())
                NotifyStopMoving(false);
        }

        public bool IsMoving()
        {
            // not sure if it is good :/
            if (Movement != null && Movement.IsEnded())
                NotifyStopMoving(false);

            return Movement != null;
        }

        public override void Dispose()
        {
            StartMoving = null;
            StopMoving = null;

            base.Dispose();
        }
    }
}