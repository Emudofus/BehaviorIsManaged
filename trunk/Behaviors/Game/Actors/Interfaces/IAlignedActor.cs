using BiM.Behaviors.Game.Alignement;

namespace BiM.Behaviors.Game.Actors.Interfaces
{
    public interface IAlignedActor
    {
        AlignmentInformations Alignement
        {
            get;
        }
    }
}