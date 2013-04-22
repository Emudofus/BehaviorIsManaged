

// Generated on 04/17/2013 22:29:46
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HouseKickIndoorMerchantRequestMessage : NetworkMessage
    {
        public const uint Id = 5661;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short cellId;
        
        public HouseKickIndoorMerchantRequestMessage()
        {
        }
        
        public HouseKickIndoorMerchantRequestMessage(short cellId)
        {
            this.cellId = cellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(cellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
        
    }
    
}