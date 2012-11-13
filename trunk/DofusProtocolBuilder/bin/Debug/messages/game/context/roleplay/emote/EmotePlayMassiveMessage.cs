

// Generated on 10/25/2012 10:42:40
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class EmotePlayMassiveMessage : EmotePlayAbstractMessage
    {
        public const uint Id = 5691;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int[] actorIds;
        
        public EmotePlayMassiveMessage()
        {
        }
        
        public EmotePlayMassiveMessage(sbyte emoteId, double emoteStartTime, int[] actorIds)
         : base(emoteId, emoteStartTime)
        {
            this.actorIds = actorIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)actorIds.Length);
            foreach (var entry in actorIds)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            actorIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 actorIds[i] = reader.ReadInt();
            }
        }
        
    }
    
}