

// Generated on 12/11/2012 19:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TaxCollectorMovementMessage : NetworkMessage
    {
        public const uint Id = 5633;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool hireOrFire;
        public Types.TaxCollectorBasicInformations basicInfos;
        public string playerName;
        
        public TaxCollectorMovementMessage()
        {
        }
        
        public TaxCollectorMovementMessage(bool hireOrFire, Types.TaxCollectorBasicInformations basicInfos, string playerName)
        {
            this.hireOrFire = hireOrFire;
            this.basicInfos = basicInfos;
            this.playerName = playerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(hireOrFire);
            basicInfos.Serialize(writer);
            writer.WriteUTF(playerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hireOrFire = reader.ReadBoolean();
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
            playerName = reader.ReadUTF();
        }
        
    }
    
}