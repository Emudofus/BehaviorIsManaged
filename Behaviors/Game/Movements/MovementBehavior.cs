#region License GNU GPL
// MovementBehavior.cs
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
using System.ComponentModel;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Movements
{
    public class MovementBehavior : INotifyPropertyChanged
    {
        /*public delegate void PathCompletedHandler(MovementBehavior path, ContextActor actor);

        public event PathCompletedHandler Completed;

        public void NotifyCompleted(ContextActor actor)
        {
            PathCompletedHandler handler = Completed;
            if (handler != null) handler(this, actor);
        }*/


        public static VelocityConfiguration FantomMovementBehavior = new VelocityConfiguration(0.000980392, 0.00117647, 0.000147059);
        public static VelocityConfiguration MountedMovementBehavior = new VelocityConfiguration(0.005, 0.00833333, 00740741);
        public static VelocityConfiguration ParableMovementBehavior = new VelocityConfiguration(0.002, 0.00222222, 0.0025);
        public static VelocityConfiguration RunningMovementBehavior = new VelocityConfiguration(0.00392157, 0.00666667, 0.00588235);
        public static VelocityConfiguration SlideMovementBehavior = new VelocityConfiguration(0.0117647, 0.02, 0.0176471);
        public static VelocityConfiguration WalkingMovementBehavior = new VelocityConfiguration(0.00196078, 0.00235294, 0.00208333);
        public static VelocityConfiguration BenchmarkMovementBehavior = new VelocityConfiguration(0.00392157, 0.00470588, 0.00588235);

        public MovementBehavior(Path path, VelocityConfiguration velocityConfiguration)
        {
            MovementPath = path;
            VelocityConfiguration = velocityConfiguration;
        }

        public VelocityConfiguration VelocityConfiguration
        {
            get;
            private set;
        }

        public bool Canceled
        {
            get;
            private set;
        }

        public double CurrentVelocity
        {
            get { return TimedPath.GetCurrentVelocity(); }
        }

        public Path MovementPath
        {
            get;
            private set;
        }

        public Cell StartCell
        {
            get { return MovementPath.Start; }
        }

        public Cell EndCell
        {
            get { return MovementPath.End; }
        }


        public DirectionsEnum EndDirection
        {
            get { return MovementPath.GetEndCellDirection(); }
        }

        public TimedPath TimedPath
        {
            get;
            private set;
        }

        public DateTime StartTime
        {
            get;
            private set;
        }

        public DateTime EndTime
        {
            get { return TimedPath.Elements[TimedPath.Elements.Count - 1].EndTime; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void Start()
        {
            Start(DateTime.Now);
        }

        public void Start(DateTime startTime)
        {
            StartTime = startTime;
            TimedPath = TimedPath.Create(MovementPath,
                                         VelocityConfiguration.HorizontalVelocity,
                                         VelocityConfiguration.VerticalVelocity,
                                         VelocityConfiguration.LinearVelocity,
                                         StartTime);
        }

        public void Cancel()
        {
            Canceled = true;
        }

        public bool IsEnded()
        {
            return DateTime.Now >= EndTime || Canceled;
        }
    }
}