

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameDataPaddockObjectListAddMessage : NetworkMessage
    {
        public const uint Id = 5992;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.PaddockItem[] paddockItemDescription;
        
        public GameDataPaddockObjectListAddMessage()
        {
        }
        
        public GameDataPaddockObjectListAddMessage(Types.PaddockItem[] paddockItemDescription)
        {
            this.paddockItemDescription = paddockItemDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)paddockItemDescription.Length);
            foreach (var entry in paddockItemDescription)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            paddockItemDescription = new Types.PaddockItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 paddockItemDescription[i] = new Types.PaddockItem();
                 paddockItemDescription[i].Deserialize(reader);
            }
        }
        
    }
    
}