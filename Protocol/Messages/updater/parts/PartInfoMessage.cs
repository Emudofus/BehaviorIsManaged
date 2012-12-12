

// Generated on 12/11/2012 19:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartInfoMessage : NetworkMessage
    {
        public const uint Id = 1508;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ContentPart part;
        public float installationPercent;
        
        public PartInfoMessage()
        {
        }
        
        public PartInfoMessage(Types.ContentPart part, float installationPercent)
        {
            this.part = part;
            this.installationPercent = installationPercent;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            part.Serialize(writer);
            writer.WriteFloat(installationPercent);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            part = new Types.ContentPart();
            part.Deserialize(reader);
            installationPercent = reader.ReadFloat();
        }
        
    }
    
}