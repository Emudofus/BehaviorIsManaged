

// Generated on 10/25/2012 10:42:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ShortcutObjectPreset : ShortcutObject
    {
        public const short Id = 370;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        
        public ShortcutObjectPreset()
        {
        }
        
        public ShortcutObjectPreset(int slot, sbyte presetId)
         : base(slot)
        {
            this.presetId = presetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(presetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
        }
        
    }
    
}