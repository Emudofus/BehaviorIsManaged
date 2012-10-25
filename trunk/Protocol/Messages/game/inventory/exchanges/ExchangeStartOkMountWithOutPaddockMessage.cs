

// Generated on 10/25/2012 10:42:51
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeStartOkMountWithOutPaddockMessage : NetworkMessage
    {
        public const uint Id = 5991;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.MountClientData[] stabledMountsDescription;
        
        public ExchangeStartOkMountWithOutPaddockMessage()
        {
        }
        
        public ExchangeStartOkMountWithOutPaddockMessage(Types.MountClientData[] stabledMountsDescription)
        {
            this.stabledMountsDescription = stabledMountsDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)stabledMountsDescription.Length);
            foreach (var entry in stabledMountsDescription)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            stabledMountsDescription = new Types.MountClientData[limit];
            for (int i = 0; i < limit; i++)
            {
                 stabledMountsDescription[i] = new Types.MountClientData();
                 stabledMountsDescription[i].Deserialize(reader);
            }
        }
        
    }
    
}