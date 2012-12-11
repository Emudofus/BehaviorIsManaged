

// Generated on 12/11/2012 19:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DownloadErrorMessage : NetworkMessage
    {
        public const uint Id = 1513;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte errorId;
        public string message;
        public string helpUrl;
        
        public DownloadErrorMessage()
        {
        }
        
        public DownloadErrorMessage(sbyte errorId, string message, string helpUrl)
        {
            this.errorId = errorId;
            this.message = message;
            this.helpUrl = helpUrl;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(errorId);
            writer.WriteUTF(message);
            writer.WriteUTF(helpUrl);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            errorId = reader.ReadSByte();
            if (errorId < 0)
                throw new Exception("Forbidden value on errorId = " + errorId + ", it doesn't respect the following condition : errorId < 0");
            message = reader.ReadUTF();
            helpUrl = reader.ReadUTF();
        }
        
    }
    
}