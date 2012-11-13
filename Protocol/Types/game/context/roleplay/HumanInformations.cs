#region License GNU GPL
// HumanInformations.cs
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
    public class HumanInformations
    {
        public const short Id = 157;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public Types.EntityLook[] followingCharactersLook;
        public sbyte emoteId;
        public double emoteStartTime;
        public Types.ActorRestrictionsInformations restrictions;
        public short titleId;
        public string titleParam;
        
        public HumanInformations()
        {
        }
        
        public HumanInformations(Types.EntityLook[] followingCharactersLook, sbyte emoteId, double emoteStartTime, Types.ActorRestrictionsInformations restrictions, short titleId, string titleParam)
        {
            this.followingCharactersLook = followingCharactersLook;
            this.emoteId = emoteId;
            this.emoteStartTime = emoteStartTime;
            this.restrictions = restrictions;
            this.titleId = titleId;
            this.titleParam = titleParam;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)followingCharactersLook.Length);
            foreach (var entry in followingCharactersLook)
            {
                 entry.Serialize(writer);
            }
            writer.WriteSByte(emoteId);
            writer.WriteDouble(emoteStartTime);
            restrictions.Serialize(writer);
            writer.WriteShort(titleId);
            writer.WriteUTF(titleParam);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            followingCharactersLook = new Types.EntityLook[limit];
            for (int i = 0; i < limit; i++)
            {
                 followingCharactersLook[i] = new Types.EntityLook();
                 followingCharactersLook[i].Deserialize(reader);
            }
            emoteId = reader.ReadSByte();
            emoteStartTime = reader.ReadDouble();
            restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
            titleId = reader.ReadShort();
            if (titleId < 0)
                throw new Exception("Forbidden value on titleId = " + titleId + ", it doesn't respect the following condition : titleId < 0");
            titleParam = reader.ReadUTF();
        }
        
    }
    
}