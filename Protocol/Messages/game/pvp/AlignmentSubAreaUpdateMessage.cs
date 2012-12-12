

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AlignmentSubAreaUpdateMessage : NetworkMessage
    {
        public const uint Id = 6057;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public sbyte side;
        public bool quiet;
        
        public AlignmentSubAreaUpdateMessage()
        {
        }
        
        public AlignmentSubAreaUpdateMessage(short subAreaId, sbyte side, bool quiet)
        {
            this.subAreaId = subAreaId;
            this.side = side;
            this.quiet = quiet;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteSByte(side);
            writer.WriteBoolean(quiet);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            side = reader.ReadSByte();
            quiet = reader.ReadBoolean();
        }
        
    }
    
}