

// Generated on 04/17/2013 22:29:38
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class BasicWhoIsMessage : NetworkMessage
    {
        public const uint Id = 180;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool self;
        public sbyte position;
        public string accountNickname;
        public int accountId;
        public string playerName;
        public int playerId;
        public short areaId;
        
        public BasicWhoIsMessage()
        {
        }
        
        public BasicWhoIsMessage(bool self, sbyte position, string accountNickname, int accountId, string playerName, int playerId, short areaId)
        {
            this.self = self;
            this.position = position;
            this.accountNickname = accountNickname;
            this.accountId = accountId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.areaId = areaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(self);
            writer.WriteSByte(position);
            writer.WriteUTF(accountNickname);
            writer.WriteInt(accountId);
            writer.WriteUTF(playerName);
            writer.WriteInt(playerId);
            writer.WriteShort(areaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            self = reader.ReadBoolean();
            position = reader.ReadSByte();
            accountNickname = reader.ReadUTF();
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            playerName = reader.ReadUTF();
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            areaId = reader.ReadShort();
        }
        
    }
    
}