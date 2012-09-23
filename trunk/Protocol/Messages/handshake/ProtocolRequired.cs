

// Generated on 09/23/2012 22:27:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ProtocolRequired : NetworkMessage
    {
        public const uint Id = 1;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int requiredVersion;
        public int currentVersion;
        
        public ProtocolRequired()
        {
        }
        
        public ProtocolRequired(int requiredVersion, int currentVersion)
        {
            this.requiredVersion = requiredVersion;
            this.currentVersion = currentVersion;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(requiredVersion);
            writer.WriteInt(currentVersion);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requiredVersion = reader.ReadInt();
            if (requiredVersion < 0)
                throw new Exception("Forbidden value on requiredVersion = " + requiredVersion + ", it doesn't respect the following condition : requiredVersion < 0");
            currentVersion = reader.ReadInt();
            if (currentVersion < 0)
                throw new Exception("Forbidden value on currentVersion = " + currentVersion + ", it doesn't respect the following condition : currentVersion < 0");
        }
        
    }
    
}