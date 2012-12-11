

// Generated on 12/11/2012 19:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ZaapListMessage : TeleportDestinationsListMessage
    {
        public const uint Id = 1604;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int spawnMapId;
        
        public ZaapListMessage()
        {
        }
        
        public ZaapListMessage(sbyte teleporterType, int[] mapIds, short[] subAreaIds, short[] costs, int spawnMapId)
         : base(teleporterType, mapIds, subAreaIds, costs)
        {
            this.spawnMapId = spawnMapId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(spawnMapId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spawnMapId = reader.ReadInt();
            if (spawnMapId < 0)
                throw new Exception("Forbidden value on spawnMapId = " + spawnMapId + ", it doesn't respect the following condition : spawnMapId < 0");
        }
        
    }
    
}