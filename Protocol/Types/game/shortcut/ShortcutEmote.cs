

// Generated on 04/17/2013 22:30:11
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ShortcutEmote : Shortcut
    {
        public const short Id = 389;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte emoteId;
        
        public ShortcutEmote()
        {
        }
        
        public ShortcutEmote(int slot, sbyte emoteId)
         : base(slot)
        {
            this.emoteId = emoteId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(emoteId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            emoteId = reader.ReadSByte();
            if (emoteId < 0)
                throw new Exception("Forbidden value on emoteId = " + emoteId + ", it doesn't respect the following condition : emoteId < 0");
        }
        
    }
    
}