namespace BiM.Game.Actors.RolePlay
{
    public abstract class NamedActor : RolePlayActor
    {
        public virtual string Name
        {
            get;
            protected set;
        }
    }
}