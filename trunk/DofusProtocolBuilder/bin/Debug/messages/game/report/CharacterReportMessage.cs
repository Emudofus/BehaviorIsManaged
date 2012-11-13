

// Generated on 10/25/2012 10:42:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterReportMessage : NetworkMessage
    {
        public const uint Id = 6079;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public uint reportedId;
        public sbyte reason;
        
        public CharacterReportMessage()
        {
        }
        
        public CharacterReportMessage(uint reportedId, sbyte reason)
        {
            this.reportedId = reportedId;
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUInt(reportedId);
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reportedId = reader.ReadUInt();
            if (reportedId < 0 || reportedId > 4294967295)
                throw new Exception("Forbidden value on reportedId = " + reportedId + ", it doesn't respect the following condition : reportedId < 0 || reportedId > 4294967295");
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
        }
        
    }
    
}