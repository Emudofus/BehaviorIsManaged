#region License GNU GPL
// CharacterExperienceGainMessage.cs
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
    public class CharacterExperienceGainMessage : NetworkMessage
    {
        public const uint Id = 6321;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double experienceCharacter;
        public double experienceMount;
        public double experienceGuild;
        public double experienceIncarnation;
        
        public CharacterExperienceGainMessage()
        {
        }
        
        public CharacterExperienceGainMessage(double experienceCharacter, double experienceMount, double experienceGuild, double experienceIncarnation)
        {
            this.experienceCharacter = experienceCharacter;
            this.experienceMount = experienceMount;
            this.experienceGuild = experienceGuild;
            this.experienceIncarnation = experienceIncarnation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(experienceCharacter);
            writer.WriteDouble(experienceMount);
            writer.WriteDouble(experienceGuild);
            writer.WriteDouble(experienceIncarnation);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            experienceCharacter = reader.ReadDouble();
            if (experienceCharacter < 0)
                throw new Exception("Forbidden value on experienceCharacter = " + experienceCharacter + ", it doesn't respect the following condition : experienceCharacter < 0");
            experienceMount = reader.ReadDouble();
            if (experienceMount < 0)
                throw new Exception("Forbidden value on experienceMount = " + experienceMount + ", it doesn't respect the following condition : experienceMount < 0");
            experienceGuild = reader.ReadDouble();
            if (experienceGuild < 0)
                throw new Exception("Forbidden value on experienceGuild = " + experienceGuild + ", it doesn't respect the following condition : experienceGuild < 0");
            experienceIncarnation = reader.ReadDouble();
            if (experienceIncarnation < 0)
                throw new Exception("Forbidden value on experienceIncarnation = " + experienceIncarnation + ", it doesn't respect the following condition : experienceIncarnation < 0");
        }
        
    }
    
}