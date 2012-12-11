

// Generated on 12/11/2012 19:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MoodSmileyResultMessage : NetworkMessage
    {
        public const uint Id = 6196;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte resultCode;
        public sbyte smileyId;
        
        public MoodSmileyResultMessage()
        {
        }
        
        public MoodSmileyResultMessage(sbyte resultCode, sbyte smileyId)
        {
            this.resultCode = resultCode;
            this.smileyId = smileyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(resultCode);
            writer.WriteSByte(smileyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            resultCode = reader.ReadSByte();
            if (resultCode < 0)
                throw new Exception("Forbidden value on resultCode = " + resultCode + ", it doesn't respect the following condition : resultCode < 0");
            smileyId = reader.ReadSByte();
        }
        
    }
    
}