

// Generated on 12/11/2012 19:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AchievementDetailedListRequestMessage : NetworkMessage
    {
        public const uint Id = 6357;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short categoryId;
        
        public AchievementDetailedListRequestMessage()
        {
        }
        
        public AchievementDetailedListRequestMessage(short categoryId)
        {
            this.categoryId = categoryId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(categoryId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            categoryId = reader.ReadShort();
            if (categoryId < 0)
                throw new Exception("Forbidden value on categoryId = " + categoryId + ", it doesn't respect the following condition : categoryId < 0");
        }
        
    }
    
}