#region License GNU GPL
// ContextActor.cs
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
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Chat;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Actors
{
    public abstract class ContextActor : WorldObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public delegate void MoveStartHandler(ContextActor actor, MovementBehavior movement);

        public event MoveStartHandler StartMoving;

        public virtual bool NotifyStartMoving(Path path)
        {
            if (path.IsEmpty())
            {
                logger.Warn("Try to start moving with an empty path");
                return false;
            }

            Movement = new MovementBehavior(path, GetAdaptedVelocity(path));
            Movement.Start();


            return NotifyStartMoving(Movement);
        }

        public virtual bool NotifyStartMoving(MovementBehavior movement)
        {
            Movement = movement;

            if (Movement.StartCell != Cell)
            {
                logger.Warn("Actor start cell incorrect for this moving path Position={0}, StartPath={1}, Path={2}", Cell, Movement.StartCell, String.Join<World.Cell>(",", Movement.MovementPath.Cells));
                Cell = Movement.StartCell;
            }

            var handler = StartMoving;
            if (handler != null) handler(this, Movement);

            return true;
        }

        public delegate void MoveStopHandler(ContextActor actor, MovementBehavior movement, bool canceled, bool refused);

        public event MoveStopHandler StopMoving;

        public virtual void OnTimedPathExpired()
        {
            NotifyStopMoving(false);
        }

        public virtual void NotifyStopMoving(bool canceled, bool refused=false)
        {
            if (Movement == null)
            {
                logger.Warn("Try to stop moving while the entity is not actually moving");
                return;
            }
            if (refused)
            {
                Movement.Cancel();

                var element = Movement.StartCell;
                Cell = Movement.StartCell;               
            }
            else
                if (canceled)
                {
                    Movement.Cancel();

                    var element = Movement.TimedPath.GetCurrentElement();
                    Cell = element.CurrentCell;
                    Direction = element.Direction;
                }
                else
                {
                    Cell = Movement.EndCell;
                    Direction = Movement.EndDirection;
                }

            var movement = Movement;

            Movement = null;

            var handler = StopMoving;
            if (handler != null) handler(this, movement, canceled, refused);
        }

        public virtual VelocityConfiguration GetAdaptedVelocity(Path path)
        {
            return MovementBehavior.WalkingMovementBehavior;
        }

        public void UpdatePosition(int cell)
        {
            UpdatePosition(Map.Cells[cell]);
        }

        public virtual void UpdatePosition(Cell cell)
        {
            Cell = cell;
        }

        public delegate void SpeakHandler(ContextActor actor, BotChatMessage message);
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>I consider that any actor can speak</remarks>
        public event SpeakHandler Speak;

        public virtual void NotifySpeak(BotChatMessage message)
        {
            SpeakHandler handler = Speak;
            if (handler != null) handler(this, message);
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
            get
            {
                return Movement.MovementPath;
            }
        }

        public virtual EntityLook Look
        {
            get;
            protected set;
        }

        public virtual IMapContext Context
        {
            get;
            protected set;
        }

        public override void Tick(int dt)
        {
            base.Tick(dt);

            if (Movement != null && Movement.IsEnded())
                OnTimedPathExpired();
        }

        public bool IsMoving()
        {
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