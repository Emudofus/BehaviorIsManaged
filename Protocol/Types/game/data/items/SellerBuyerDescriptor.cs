

// Generated on 04/17/2013 22:30:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class SellerBuyerDescriptor
    {
        public const short Id = 121;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int[] quantities;
        public int[] types;
        public float taxPercentage;
        public int maxItemLevel;
        public int maxItemPerAccount;
        public int npcContextualId;
        public short unsoldDelay;
        
        public SellerBuyerDescriptor()
        {
        }
        
        public SellerBuyerDescriptor(int[] quantities, int[] types, float taxPercentage, int maxItemLevel, int maxItemPerAccount, int npcContextualId, short unsoldDelay)
        {
            this.quantities = quantities;
            this.types = types;
            this.taxPercentage = taxPercentage;
            this.maxItemLevel = maxItemLevel;
            this.maxItemPerAccount = maxItemPerAccount;
            this.npcContextualId = npcContextualId;
            this.unsoldDelay = unsoldDelay;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)quantities.Length);
            foreach (var entry in quantities)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)types.Length);
            foreach (var entry in types)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteFloat(taxPercentage);
            writer.WriteInt(maxItemLevel);
            writer.WriteInt(maxItemPerAccount);
            writer.WriteInt(npcContextualId);
            writer.WriteShort(unsoldDelay);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            quantities = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 quantities[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            types = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 types[i] = reader.ReadInt();
            }
            taxPercentage = reader.ReadFloat();
            maxItemLevel = reader.ReadInt();
            if (maxItemLevel < 0)
                throw new Exception("Forbidden value on maxItemLevel = " + maxItemLevel + ", it doesn't respect the following condition : maxItemLevel < 0");
            maxItemPerAccount = reader.ReadInt();
            if (maxItemPerAccount < 0)
                throw new Exception("Forbidden value on maxItemPerAccount = " + maxItemPerAccount + ", it doesn't respect the following condition : maxItemPerAccount < 0");
            npcContextualId = reader.ReadInt();
            unsoldDelay = reader.ReadShort();
            if (unsoldDelay < 0)
                throw new Exception("Forbidden value on unsoldDelay = " + unsoldDelay + ", it doesn't respect the following condition : unsoldDelay < 0");
        }
        
    }
    
}