

// Generated on 09/23/2012 22:26:49
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameMapChangeOrientationsMessage : NetworkMessage
    {
        public const uint Id = 6155;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ActorOrientation[] orientations;
        
        public GameMapChangeOrientationsMessage()
        {
        }
        
        public GameMapChangeOrientationsMessage(Types.ActorOrientation[] orientations)
        {
            this.orientations = orientations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)orientations.Length);
            foreach (var entry in orientations)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            orientations = new Types.ActorOrientation[limit];
            for (int i = 0; i < limit; i++)
            {
                 orientations[i] = new Types.ActorOrientation();
                 orientations[i].Deserialize(reader);
            }
        }
        
    }
    
}