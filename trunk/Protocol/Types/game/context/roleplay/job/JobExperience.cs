#region License GNU GPL
// JobExperience.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class JobExperience
    {
        public const short Id = 98;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        public sbyte jobLevel;
        public double jobXP;
        public double jobXpLevelFloor;
        public double jobXpNextLevelFloor;
        
        public JobExperience()
        {
        }
        
        public JobExperience(sbyte jobId, sbyte jobLevel, double jobXP, double jobXpLevelFloor, double jobXpNextLevelFloor)
        {
            this.jobId = jobId;
            this.jobLevel = jobLevel;
            this.jobXP = jobXP;
            this.jobXpLevelFloor = jobXpLevelFloor;
            this.jobXpNextLevelFloor = jobXpNextLevelFloor;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteSByte(jobLevel);
            writer.WriteDouble(jobXP);
            writer.WriteDouble(jobXpLevelFloor);
            writer.WriteDouble(jobXpNextLevelFloor);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            jobLevel = reader.ReadSByte();
            if (jobLevel < 0)
                throw new Exception("Forbidden value on jobLevel = " + jobLevel + ", it doesn't respect the following condition : jobLevel < 0");
            jobXP = reader.ReadDouble();
            if (jobXP < 0)
                throw new Exception("Forbidden value on jobXP = " + jobXP + ", it doesn't respect the following condition : jobXP < 0");
            jobXpLevelFloor = reader.ReadDouble();
            if (jobXpLevelFloor < 0)
                throw new Exception("Forbidden value on jobXpLevelFloor = " + jobXpLevelFloor + ", it doesn't respect the following condition : jobXpLevelFloor < 0");
            jobXpNextLevelFloor = reader.ReadDouble();
            if (jobXpNextLevelFloor < 0)
                throw new Exception("Forbidden value on jobXpNextLevelFloor = " + jobXpNextLevelFloor + ", it doesn't respect the following condition : jobXpNextLevelFloor < 0");
        }
        
    }
    
}