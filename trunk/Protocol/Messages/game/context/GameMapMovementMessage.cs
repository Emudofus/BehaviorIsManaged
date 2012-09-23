

// Generated on 09/23/2012 22:26:49
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameMapMovementMessage : NetworkMessage
    {
        public const uint Id = 951;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] keyMovements;
        public int actorId;
        
        public GameMapMovementMessage()
        {
        }
        
        public GameMapMovementMessage(short[] keyMovements, int actorId)
        {
            this.keyMovements = keyMovements;
            this.actorId = actorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)keyMovements.Length);
            foreach (var entry in keyMovements)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteInt(actorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            keyMovements = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 keyMovements[i] = reader.ReadShort();
            }
            actorId = reader.ReadInt();
        }
        
    }
    
}