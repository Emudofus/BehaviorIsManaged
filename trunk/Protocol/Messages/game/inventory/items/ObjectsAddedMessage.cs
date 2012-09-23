

// Generated on 09/23/2012 22:27:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ObjectsAddedMessage : NetworkMessage
    {
        public const uint Id = 6033;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItem[] @object;
        
        public ObjectsAddedMessage()
        {
        }
        
        public ObjectsAddedMessage(Types.ObjectItem[] @object)
        {
            this.@object = @object;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)@object.Length);
            foreach (var entry in @object)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            @object = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 @object[i] = new Types.ObjectItem();
                 @object[i].Deserialize(reader);
            }
        }
        
    }
    
}