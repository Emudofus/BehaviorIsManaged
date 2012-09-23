

// Generated on 09/23/2012 22:26:49
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameEntityDispositionMessage : NetworkMessage
    {
        public const uint Id = 5693;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.IdentifiedEntityDispositionInformations disposition;
        
        public GameEntityDispositionMessage()
        {
        }
        
        public GameEntityDispositionMessage(Types.IdentifiedEntityDispositionInformations disposition)
        {
            this.disposition = disposition;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            disposition.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            disposition = new Types.IdentifiedEntityDispositionInformations();
            disposition.Deserialize(reader);
        }
        
    }
    
}