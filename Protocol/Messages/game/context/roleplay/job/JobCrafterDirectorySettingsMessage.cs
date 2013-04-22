

// Generated on 04/17/2013 22:29:47
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class JobCrafterDirectorySettingsMessage : NetworkMessage
    {
        public const uint Id = 5652;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.JobCrafterDirectorySettings[] craftersSettings;
        
        public JobCrafterDirectorySettingsMessage()
        {
        }
        
        public JobCrafterDirectorySettingsMessage(Types.JobCrafterDirectorySettings[] craftersSettings)
        {
            this.craftersSettings = craftersSettings;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)craftersSettings.Length);
            foreach (var entry in craftersSettings)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            craftersSettings = new Types.JobCrafterDirectorySettings[limit];
            for (int i = 0; i < limit; i++)
            {
                 craftersSettings[i] = new Types.JobCrafterDirectorySettings();
                 craftersSettings[i].Deserialize(reader);
            }
        }
        
    }
    
}