

// Generated on 09/23/2012 22:27:03
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ObjectJobAddedMessage : NetworkMessage
    {
        public const uint Id = 6014;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        
        public ObjectJobAddedMessage()
        {
        }
        
        public ObjectJobAddedMessage(sbyte jobId)
        {
            this.jobId = jobId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
        }
        
    }
    
}