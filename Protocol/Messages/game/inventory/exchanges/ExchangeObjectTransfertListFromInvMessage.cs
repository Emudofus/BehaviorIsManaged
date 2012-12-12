

// Generated on 12/11/2012 19:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeObjectTransfertListFromInvMessage : NetworkMessage
    {
        public const uint Id = 6183;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int[] ids;
        
        public ExchangeObjectTransfertListFromInvMessage()
        {
        }
        
        public ExchangeObjectTransfertListFromInvMessage(int[] ids)
        {
            this.ids = ids;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)ids.Length);
            foreach (var entry in ids)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            ids = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids[i] = reader.ReadInt();
            }
        }
        
    }
    
}