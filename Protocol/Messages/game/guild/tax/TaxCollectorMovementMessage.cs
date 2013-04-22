

// Generated on 04/17/2013 22:29:55
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
        public int playerId;
        public string playerName;
        
        public TaxCollectorMovementMessage()
        {
        }
        
        public TaxCollectorMovementMessage(bool hireOrFire, Types.TaxCollectorBasicInformations basicInfos, int playerId, string playerName)
        {
            this.hireOrFire = hireOrFire;
            this.basicInfos = basicInfos;
            this.playerId = playerId;
            this.playerName = playerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(hireOrFire);
            basicInfos.Serialize(writer);
            writer.WriteInt(playerId);
            writer.WriteUTF(playerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hireOrFire = reader.ReadBoolean();
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            playerName = reader.ReadUTF();
        }
        
    }
    
}