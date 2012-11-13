#region License GNU GPL
// RolePlayActor.cs
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
using BiM.Behaviors.Game.Interactives;
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

        public delegate void UseInteractiveHandler(RolePlayActor actor, InteractiveObject interactive, InteractiveSkill skill, DateTime? usageEndTime);
        public event UseInteractiveHandler StartUsingInteractive;

        public delegate void InteractiveUsageEndedHandler(RolePlayActor actor, InteractiveObject interactive, InteractiveSkill skill);
        public event InteractiveUsageEndedHandler StopUsingInteractive;

        public virtual void NotifyInteractiveUseEnded()
        {
            var skill = UsingSkill;
            var interactive = UsingInteractive;
            UsageEndTime = null;
            UsingSkill = null;
            UsingInteractive = null;

            InteractiveUsageEndedHandler handler = StopUsingInteractive;
            if (handler != null) handler(this, interactive, skill);
        }

        public virtual void NotifyUseInteractive(InteractiveObject interactive, InteractiveSkill skill, int duration)
        {
            if (duration > 0)
            {
                UsingInteractive = interactive;
                UsingSkill = skill;
                UsageEndTime = DateTime.Now + TimeSpan.FromMilliseconds(duration);
            }

            UseInteractiveHandler handler = StartUsingInteractive;
            if (handler != null) handler(this, interactive, skill, UsageEndTime);
        }

        public override int Id
        {
            get;
            protected set;
        }

        public InteractiveSkill UsingSkill
        {
            get;
            private set;
        }

        public InteractiveObject UsingInteractive
        {
            get;
            private set;
        }

        public DateTime? UsageEndTime
        {
            get;
            private set;
        }

        public bool IsUsingInteractive()
        {
            return UsingInteractive != null;
        }

        public override IContext Context
        {
            get;
            protected set;
        }

        public override void Tick(int dt)
        {
            base.Tick(dt);
        }
       
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}