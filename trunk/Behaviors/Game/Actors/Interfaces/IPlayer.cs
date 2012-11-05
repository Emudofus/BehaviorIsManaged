using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors
{
    public interface IPlayer
    {
        int Id
        {
            get;
        }

        string Name
        {
            get;
        }

        EntityLook Look
        {
            get;
        }
    }
}