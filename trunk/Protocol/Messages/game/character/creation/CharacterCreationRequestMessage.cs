#region License GNU GPL
// CharacterCreationRequestMessage.cs
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
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterCreationRequestMessage : NetworkMessage
    {
        public const uint Id = 160;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public sbyte breed;
        public bool sex;
        public int[] colors;
        
        public CharacterCreationRequestMessage()
        {
        }
        
        public CharacterCreationRequestMessage(string name, sbyte breed, bool sex, int[] colors)
        {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.colors = colors;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            breed = reader.ReadSByte();
            if (breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Steamer)
                throw new Exception("Forbidden value on breed = " + breed + ", it doesn't respect the following condition : breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Steamer");
            sex = reader.ReadBoolean();
            colors = new int[5];
            for (int i = 0; i < 5; i++)
            {
                 colors[i] = reader.ReadInt();
            }
        }
        
    }
    
}