

// Generated on 12/11/2012 19:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class JobExperienceMultiUpdateMessage : NetworkMessage
    {
        public const uint Id = 5809;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.JobExperience[] experiencesUpdate;
        
        public JobExperienceMultiUpdateMessage()
        {
        }
        
        public JobExperienceMultiUpdateMessage(Types.JobExperience[] experiencesUpdate)
        {
            this.experiencesUpdate = experiencesUpdate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)experiencesUpdate.Length);
            foreach (var entry in experiencesUpdate)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            experiencesUpdate = new Types.JobExperience[limit];
            for (int i = 0; i < limit; i++)
            {
                 experiencesUpdate[i] = new Types.JobExperience();
                 experiencesUpdate[i].Deserialize(reader);
            }
        }
        
    }
    
}