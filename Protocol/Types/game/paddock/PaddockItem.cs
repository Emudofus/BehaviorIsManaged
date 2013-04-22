

// Generated on 04/17/2013 22:30:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class PaddockItem : ObjectItemInRolePlay
    {
        public const short Id = 185;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.ItemDurability durability;
        
        public PaddockItem()
        {
        }
        
        public PaddockItem(short cellId, short objectGID, Types.ItemDurability durability)
         : base(cellId, objectGID)
        {
            this.durability = durability;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            durability.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            durability = new Types.ItemDurability();
            durability.Deserialize(reader);
        }
        
    }
    
}