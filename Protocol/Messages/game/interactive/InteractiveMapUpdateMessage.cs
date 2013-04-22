

// Generated on 04/17/2013 22:29:55
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class InteractiveMapUpdateMessage : NetworkMessage
    {
        public const uint Id = 5002;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.InteractiveElement[] interactiveElements;
        
        public InteractiveMapUpdateMessage()
        {
        }
        
        public InteractiveMapUpdateMessage(Types.InteractiveElement[] interactiveElements)
        {
            this.interactiveElements = interactiveElements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)interactiveElements.Length);
            foreach (var entry in interactiveElements)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            interactiveElements = new Types.InteractiveElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 interactiveElements[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElement>(reader.ReadShort());
                 interactiveElements[i].Deserialize(reader);
            }
        }
        
    }
    
}