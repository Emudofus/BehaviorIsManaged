using BiM.Core.IO;

namespace BiM.Core.Network
{
    public interface IMessageBuilder
    {
        NetworkMessage BuildMessage(uint messageid, IDataReader reader); 
    }
}