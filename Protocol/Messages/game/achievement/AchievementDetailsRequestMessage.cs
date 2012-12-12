

// Generated on 12/11/2012 19:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AchievementDetailsRequestMessage : NetworkMessage
    {
        public const uint Id = 6380;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short achievementId;
        
        public AchievementDetailsRequestMessage()
        {
        }
        
        public AchievementDetailsRequestMessage(short achievementId)
        {
            this.achievementId = achievementId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(achievementId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            achievementId = reader.ReadShort();
            if (achievementId < 0)
                throw new Exception("Forbidden value on achievementId = " + achievementId + ", it doesn't respect the following condition : achievementId < 0");
        }
        
    }
    
}