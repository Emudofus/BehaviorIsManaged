

// Generated on 12/11/2012 19:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DownloadSetSpeedRequestMessage : NetworkMessage
    {
        public const uint Id = 1512;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte downloadSpeed;
        
        public DownloadSetSpeedRequestMessage()
        {
        }
        
        public DownloadSetSpeedRequestMessage(sbyte downloadSpeed)
        {
            this.downloadSpeed = downloadSpeed;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(downloadSpeed);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            downloadSpeed = reader.ReadSByte();
            if (downloadSpeed < 1 || downloadSpeed > 10)
                throw new Exception("Forbidden value on downloadSpeed = " + downloadSpeed + ", it doesn't respect the following condition : downloadSpeed < 1 || downloadSpeed > 10");
        }
        
    }
    
}