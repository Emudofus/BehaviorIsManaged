

// Generated on 10/25/2012 10:42:40
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MapRunningFightListMessage : NetworkMessage
    {
        public const uint Id = 5743;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FightExternalInformations[] fights;
        
        public MapRunningFightListMessage()
        {
        }
        
        public MapRunningFightListMessage(Types.FightExternalInformations[] fights)
        {
            this.fights = fights;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)fights.Length);
            foreach (var entry in fights)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            fights = new Types.FightExternalInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fights[i] = new Types.FightExternalInformations();
                 fights[i].Deserialize(reader);
            }
        }
        
    }
    
}