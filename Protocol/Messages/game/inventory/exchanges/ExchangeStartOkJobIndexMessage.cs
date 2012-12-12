

// Generated on 12/11/2012 19:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeStartOkJobIndexMessage : NetworkMessage
    {
        public const uint Id = 5819;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int[] jobs;
        
        public ExchangeStartOkJobIndexMessage()
        {
        }
        
        public ExchangeStartOkJobIndexMessage(int[] jobs)
        {
            this.jobs = jobs;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)jobs.Length);
            foreach (var entry in jobs)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            jobs = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 jobs[i] = reader.ReadInt();
            }
        }
        
    }
    
}