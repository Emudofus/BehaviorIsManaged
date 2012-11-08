#region License GNU GPL
// InteractiveElement.cs
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
    public class InteractiveElement
    {
        public const short Id = 80;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int elementId;
        public int elementTypeId;
        public Types.InteractiveElementSkill[] enabledSkills;
        public Types.InteractiveElementSkill[] disabledSkills;
        
        public InteractiveElement()
        {
        }
        
        public InteractiveElement(int elementId, int elementTypeId, Types.InteractiveElementSkill[] enabledSkills, Types.InteractiveElementSkill[] disabledSkills)
        {
            this.elementId = elementId;
            this.elementTypeId = elementTypeId;
            this.enabledSkills = enabledSkills;
            this.disabledSkills = disabledSkills;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(elementId);
            writer.WriteInt(elementTypeId);
            writer.WriteUShort((ushort)enabledSkills.Length);
            foreach (var entry in enabledSkills)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)disabledSkills.Length);
            foreach (var entry in disabledSkills)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
            elementTypeId = reader.ReadInt();
            var limit = reader.ReadUShort();
            enabledSkills = new Types.InteractiveElementSkill[limit];
            for (int i = 0; i < limit; i++)
            {
                 enabledSkills[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElementSkill>(reader.ReadShort());
                 enabledSkills[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            disabledSkills = new Types.InteractiveElementSkill[limit];
            for (int i = 0; i < limit; i++)
            {
                 disabledSkills[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElementSkill>(reader.ReadShort());
                 disabledSkills[i].Deserialize(reader);
            }
        }
        
    }
    
}