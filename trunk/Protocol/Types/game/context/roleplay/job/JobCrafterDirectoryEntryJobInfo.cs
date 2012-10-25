

// Generated on 10/25/2012 10:42:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class JobCrafterDirectoryEntryJobInfo
    {
        public const short Id = 195;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        public sbyte jobLevel;
        public sbyte userDefinedParams;
        public sbyte minSlots;
        
        public JobCrafterDirectoryEntryJobInfo()
        {
        }
        
        public JobCrafterDirectoryEntryJobInfo(sbyte jobId, sbyte jobLevel, sbyte userDefinedParams, sbyte minSlots)
        {
            this.jobId = jobId;
            this.jobLevel = jobLevel;
            this.userDefinedParams = userDefinedParams;
            this.minSlots = minSlots;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteSByte(jobLevel);
            writer.WriteSByte(userDefinedParams);
            writer.WriteSByte(minSlots);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            jobLevel = reader.ReadSByte();
            if (jobLevel < 1 || jobLevel > 100)
                throw new Exception("Forbidden value on jobLevel = " + jobLevel + ", it doesn't respect the following condition : jobLevel < 1 || jobLevel > 100");
            userDefinedParams = reader.ReadSByte();
            if (userDefinedParams < 0)
                throw new Exception("Forbidden value on userDefinedParams = " + userDefinedParams + ", it doesn't respect the following condition : userDefinedParams < 0");
            minSlots = reader.ReadSByte();
            if (minSlots < 0 || minSlots > 9)
                throw new Exception("Forbidden value on minSlots = " + minSlots + ", it doesn't respect the following condition : minSlots < 0 || minSlots > 9");
        }
        
    }
    
}