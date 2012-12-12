

// Generated on 12/11/2012 19:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class StorageObjectsUpdateMessage : NetworkMessage
    {
        public const uint Id = 6036;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItem[] objectList;
        
        public StorageObjectsUpdateMessage()
        {
        }
        
        public StorageObjectsUpdateMessage(Types.ObjectItem[] objectList)
        {
            this.objectList = objectList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)objectList.Length);
            foreach (var entry in objectList)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            objectList = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectList[i] = new Types.ObjectItem();
                 objectList[i].Deserialize(reader);
            }
        }
        
    }
    
}