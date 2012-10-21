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