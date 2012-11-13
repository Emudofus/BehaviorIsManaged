

// Generated on 10/25/2012 10:42:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AchievementUpdateMessage : NetworkMessage
    {
        public const uint Id = 6207;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.Achievement achievement;
        
        public AchievementUpdateMessage()
        {
        }
        
        public AchievementUpdateMessage(Types.Achievement achievement)
        {
            this.achievement = achievement;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            achievement.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            achievement = new Types.Achievement();
            achievement.Deserialize(reader);
        }
        
    }
    
}