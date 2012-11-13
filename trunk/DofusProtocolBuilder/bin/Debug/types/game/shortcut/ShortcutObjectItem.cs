

// Generated on 10/25/2012 10:42:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ShortcutObjectItem : ShortcutObject
    {
        public const short Id = 371;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int itemUID;
        public int itemGID;
        
        public ShortcutObjectItem()
        {
        }
        
        public ShortcutObjectItem(int slot, int itemUID, int itemGID)
         : base(slot)
        {
            this.itemUID = itemUID;
            this.itemGID = itemGID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(itemUID);
            writer.WriteInt(itemGID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            itemUID = reader.ReadInt();
            itemGID = reader.ReadInt();
        }
        
    }
    
}