

// Generated on 04/17/2013 22:30:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HumanOptionEmote : HumanOption
    {
        public const short Id = 407;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte emoteId;
        public double emoteStartTime;
        
        public HumanOptionEmote()
        {
        }
        
        public HumanOptionEmote(sbyte emoteId, double emoteStartTime)
        {
            this.emoteId = emoteId;
            this.emoteStartTime = emoteStartTime;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(emoteId);
            writer.WriteDouble(emoteStartTime);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            emoteId = reader.ReadSByte();
            emoteStartTime = reader.ReadDouble();
        }
        
    }
    
}