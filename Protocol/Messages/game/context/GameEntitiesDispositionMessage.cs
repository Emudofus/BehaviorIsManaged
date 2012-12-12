

// Generated on 12/11/2012 19:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameEntitiesDispositionMessage : NetworkMessage
    {
        public const uint Id = 5696;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.IdentifiedEntityDispositionInformations[] dispositions;
        
        public GameEntitiesDispositionMessage()
        {
        }
        
        public GameEntitiesDispositionMessage(Types.IdentifiedEntityDispositionInformations[] dispositions)
        {
            this.dispositions = dispositions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)dispositions.Length);
            foreach (var entry in dispositions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            dispositions = new Types.IdentifiedEntityDispositionInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 dispositions[i] = new Types.IdentifiedEntityDispositionInformations();
                 dispositions[i].Deserialize(reader);
            }
        }
        
    }
    
}