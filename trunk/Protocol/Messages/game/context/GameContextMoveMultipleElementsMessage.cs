

// Generated on 09/23/2012 22:26:49
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameContextMoveMultipleElementsMessage : NetworkMessage
    {
        public const uint Id = 254;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.EntityMovementInformations[] movements;
        
        public GameContextMoveMultipleElementsMessage()
        {
        }
        
        public GameContextMoveMultipleElementsMessage(Types.EntityMovementInformations[] movements)
        {
            this.movements = movements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)movements.Length);
            foreach (var entry in movements)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            movements = new Types.EntityMovementInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 movements[i] = new Types.EntityMovementInformations();
                 movements[i].Deserialize(reader);
            }
        }
        
    }
    
}