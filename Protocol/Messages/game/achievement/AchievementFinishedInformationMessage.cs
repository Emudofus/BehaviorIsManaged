

// Generated on 12/11/2012 19:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AchievementFinishedInformationMessage : AchievementFinishedMessage
    {
        public const uint Id = 6381;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public int playerId;
        
        public AchievementFinishedInformationMessage()
        {
        }
        
        public AchievementFinishedInformationMessage(short id, short finishedlevel, string name, int playerId)
         : base(id, finishedlevel)
        {
            this.name = name;
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
            writer.WriteInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
    }
    
}