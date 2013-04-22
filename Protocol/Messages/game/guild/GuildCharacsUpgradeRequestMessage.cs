

// Generated on 04/17/2013 22:29:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildCharacsUpgradeRequestMessage : NetworkMessage
    {
        public const uint Id = 5706;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte charaTypeTarget;
        
        public GuildCharacsUpgradeRequestMessage()
        {
        }
        
        public GuildCharacsUpgradeRequestMessage(sbyte charaTypeTarget)
        {
            this.charaTypeTarget = charaTypeTarget;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(charaTypeTarget);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            charaTypeTarget = reader.ReadSByte();
            if (charaTypeTarget < 0)
                throw new Exception("Forbidden value on charaTypeTarget = " + charaTypeTarget + ", it doesn't respect the following condition : charaTypeTarget < 0");
        }
        
    }
    
}