

// Generated on 12/11/2012 19:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class JobCrafterDirectoryDefineSettingsMessage : NetworkMessage
    {
        public const uint Id = 5649;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.JobCrafterDirectorySettings settings;
        
        public JobCrafterDirectoryDefineSettingsMessage()
        {
        }
        
        public JobCrafterDirectoryDefineSettingsMessage(Types.JobCrafterDirectorySettings settings)
        {
            this.settings = settings;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            settings.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            settings = new Types.JobCrafterDirectorySettings();
            settings.Deserialize(reader);
        }
        
    }
    
}