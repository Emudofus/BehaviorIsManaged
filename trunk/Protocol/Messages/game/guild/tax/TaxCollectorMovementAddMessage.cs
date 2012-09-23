

// Generated on 09/23/2012 22:26:59
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TaxCollectorMovementAddMessage : NetworkMessage
    {
        public const uint Id = 5917;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.TaxCollectorInformations informations;
        
        public TaxCollectorMovementAddMessage()
        {
        }
        
        public TaxCollectorMovementAddMessage(Types.TaxCollectorInformations informations)
        {
            this.informations = informations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(informations.TypeId);
            informations.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            informations = Types.ProtocolTypeManager.GetInstance<Types.TaxCollectorInformations>(reader.ReadShort());
            informations.Deserialize(reader);
        }
        
    }
    
}