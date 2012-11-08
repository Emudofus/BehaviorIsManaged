#region License GNU GPL
// JobCrafterDirectorySettings.cs
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
    public class JobCrafterDirectorySettings
    {
        public const short Id = 97;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        public sbyte minSlot;
        public sbyte userDefinedParams;
        
        public JobCrafterDirectorySettings()
        {
        }
        
        public JobCrafterDirectorySettings(sbyte jobId, sbyte minSlot, sbyte userDefinedParams)
        {
            this.jobId = jobId;
            this.minSlot = minSlot;
            this.userDefinedParams = userDefinedParams;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteSByte(minSlot);
            writer.WriteSByte(userDefinedParams);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            minSlot = reader.ReadSByte();
            if (minSlot < 0 || minSlot > 9)
                throw new Exception("Forbidden value on minSlot = " + minSlot + ", it doesn't respect the following condition : minSlot < 0 || minSlot > 9");
            userDefinedParams = reader.ReadSByte();
            if (userDefinedParams < 0)
                throw new Exception("Forbidden value on userDefinedParams = " + userDefinedParams + ", it doesn't respect the following condition : userDefinedParams < 0");
        }
        
    }
    
}