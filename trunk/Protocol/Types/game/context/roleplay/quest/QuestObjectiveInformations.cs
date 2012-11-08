#region License GNU GPL
// QuestObjectiveInformations.cs
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
    public class QuestObjectiveInformations
    {
        public const short Id = 385;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short objectiveId;
        public bool objectiveStatus;
        
        public QuestObjectiveInformations()
        {
        }
        
        public QuestObjectiveInformations(short objectiveId, bool objectiveStatus)
        {
            this.objectiveId = objectiveId;
            this.objectiveStatus = objectiveStatus;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(objectiveId);
            writer.WriteBoolean(objectiveStatus);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            objectiveId = reader.ReadShort();
            if (objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
            objectiveStatus = reader.ReadBoolean();
        }
        
    }
    
}