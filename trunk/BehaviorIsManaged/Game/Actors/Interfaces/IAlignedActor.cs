using BiM.Game.Alignement;
using BiM.Protocol.Types;

namespace BiM.Game.Actors.Interfaces
{
    public interface IAlignedActor
    {
        AlignmentInformations Alignement
        {
            get;
        }
    }
}