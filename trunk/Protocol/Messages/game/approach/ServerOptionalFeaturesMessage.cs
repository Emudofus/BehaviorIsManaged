

// Generated on 09/23/2012 22:26:46
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ServerOptionalFeaturesMessage : NetworkMessage
    {
        public const uint Id = 6305;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] features;
        
        public ServerOptionalFeaturesMessage()
        {
        }
        
        public ServerOptionalFeaturesMessage(short[] features)
        {
            this.features = features;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)features.Length);
            foreach (var entry in features)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            features = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 features[i] = reader.ReadShort();
            }
        }
        
    }
    
}