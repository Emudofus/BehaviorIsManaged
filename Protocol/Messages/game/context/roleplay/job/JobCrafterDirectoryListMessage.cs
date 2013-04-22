

// Generated on 04/17/2013 22:29:47
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class JobCrafterDirectoryListMessage : NetworkMessage
    {
        public const uint Id = 6046;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.JobCrafterDirectoryListEntry[] listEntries;
        
        public JobCrafterDirectoryListMessage()
        {
        }
        
        public JobCrafterDirectoryListMessage(Types.JobCrafterDirectoryListEntry[] listEntries)
        {
            this.listEntries = listEntries;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)listEntries.Length);
            foreach (var entry in listEntries)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            listEntries = new Types.JobCrafterDirectoryListEntry[limit];
            for (int i = 0; i < limit; i++)
            {
                 listEntries[i] = new Types.JobCrafterDirectoryListEntry();
                 listEntries[i].Deserialize(reader);
            }
        }
        
    }
    
}