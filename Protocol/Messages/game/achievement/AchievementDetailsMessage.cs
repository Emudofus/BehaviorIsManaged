

// Generated on 12/11/2012 19:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AchievementDetailsMessage : NetworkMessage
    {
        public const uint Id = 6378;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.Achievement achievement;
        
        public AchievementDetailsMessage()
        {
        }
        
        public AchievementDetailsMessage(Types.Achievement achievement)
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