

// Generated on 04/17/2013 22:29:41
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MoodSmileyUpdateMessage : NetworkMessage
    {
        public const uint Id = 6388;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int accountId;
        public int playerId;
        public sbyte smileyId;
        
        public MoodSmileyUpdateMessage()
        {
        }
        
        public MoodSmileyUpdateMessage(int accountId, int playerId, sbyte smileyId)
        {
            this.accountId = accountId;
            this.playerId = playerId;
            this.smileyId = smileyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(accountId);
            writer.WriteInt(playerId);
            writer.WriteSByte(smileyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            smileyId = reader.ReadSByte();
        }
        
    }
    
}