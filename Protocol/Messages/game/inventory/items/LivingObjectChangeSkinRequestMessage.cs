

// Generated on 04/17/2013 22:30:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class LivingObjectChangeSkinRequestMessage : NetworkMessage
    {
        public const uint Id = 5725;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int livingUID;
        public byte livingPosition;
        public int skinId;
        
        public LivingObjectChangeSkinRequestMessage()
        {
        }
        
        public LivingObjectChangeSkinRequestMessage(int livingUID, byte livingPosition, int skinId)
        {
            this.livingUID = livingUID;
            this.livingPosition = livingPosition;
            this.skinId = skinId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(livingUID);
            writer.WriteByte(livingPosition);
            writer.WriteInt(skinId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            livingUID = reader.ReadInt();
            if (livingUID < 0)
                throw new Exception("Forbidden value on livingUID = " + livingUID + ", it doesn't respect the following condition : livingUID < 0");
            livingPosition = reader.ReadByte();
            if (livingPosition < 0 || livingPosition > 255)
                throw new Exception("Forbidden value on livingPosition = " + livingPosition + ", it doesn't respect the following condition : livingPosition < 0 || livingPosition > 255");
            skinId = reader.ReadInt();
            if (skinId < 0)
                throw new Exception("Forbidden value on skinId = " + skinId + ", it doesn't respect the following condition : skinId < 0");
        }
        
    }
    
}