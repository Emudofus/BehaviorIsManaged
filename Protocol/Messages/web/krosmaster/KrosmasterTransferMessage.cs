

// Generated on 12/11/2012 19:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class KrosmasterTransferMessage : NetworkMessage
    {
        public const uint Id = 6348;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string uid;
        public sbyte failure;
        
        public KrosmasterTransferMessage()
        {
        }
        
        public KrosmasterTransferMessage(string uid, sbyte failure)
        {
            this.uid = uid;
            this.failure = failure;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(uid);
            writer.WriteSByte(failure);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            uid = reader.ReadUTF();
            failure = reader.ReadSByte();
            if (failure < 0)
                throw new Exception("Forbidden value on failure = " + failure + ", it doesn't respect the following condition : failure < 0");
        }
        
    }
    
}