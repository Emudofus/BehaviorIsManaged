using BiM.Behaviors.Game.Actors.Interfaces;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public abstract class NamedActor : RolePlayActor, INamed
    {
        public virtual string Name
        {
            get;
            protected set;
        }
    }
}