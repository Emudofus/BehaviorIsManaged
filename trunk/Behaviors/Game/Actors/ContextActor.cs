using BiM.Behaviors.Game.Chat;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors
{
    public abstract class ContextActor : WorldObject
    {
        public delegate void SpeakHandler(ContextActor actor, ChatMessage message);
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>I consider that any actor can speak</remarks>
        public event SpeakHandler Speak;

        public virtual void NotifySpeak(ChatMessage message)
        {
            SpeakHandler handler = Speak;
            if (handler != null) handler(this, message);
        }

        public virtual EntityLook Look
        {
            get;
            set;
        }

        public virtual IContext Context
        {
            get;
            protected set;
        }
    }
}