

// Generated on 10/25/2012 10:42:47
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TaxCollectorAttackedResultMessage : NetworkMessage
    {
        public const uint Id = 5635;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool deadOrAlive;
        public Types.TaxCollectorBasicInformations basicInfos;
        
        public TaxCollectorAttackedResultMessage()
        {
        }
        
        public TaxCollectorAttackedResultMessage(bool deadOrAlive, Types.TaxCollectorBasicInformations basicInfos)
        {
            this.deadOrAlive = deadOrAlive;
            this.basicInfos = basicInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(deadOrAlive);
            basicInfos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            deadOrAlive = reader.ReadBoolean();
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
        }
        
    }
    
}