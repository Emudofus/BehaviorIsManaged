

// Generated on 04/17/2013 22:29:46
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HousePropertiesMessage : NetworkMessage
    {
        public const uint Id = 5734;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.HouseInformations properties;
        
        public HousePropertiesMessage()
        {
        }
        
        public HousePropertiesMessage(Types.HouseInformations properties)
        {
            this.properties = properties;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(properties.TypeId);
            properties.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            properties = Types.ProtocolTypeManager.GetInstance<Types.HouseInformations>(reader.ReadShort());
            properties.Deserialize(reader);
        }
        
    }
    
}