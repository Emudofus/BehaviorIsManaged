#region License GNU GPL
// ChallengeInfoMessage.cs
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
    public class ChallengeInfoMessage : NetworkMessage
    {
        public const uint Id = 6022;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte challengeId;
        public int targetId;
        public int baseXpBonus;
        public int extraXpBonus;
        public int baseDropBonus;
        public int extraDropBonus;
        
        public ChallengeInfoMessage()
        {
        }
        
        public ChallengeInfoMessage(sbyte challengeId, int targetId, int baseXpBonus, int extraXpBonus, int baseDropBonus, int extraDropBonus)
        {
            this.challengeId = challengeId;
            this.targetId = targetId;
            this.baseXpBonus = baseXpBonus;
            this.extraXpBonus = extraXpBonus;
            this.baseDropBonus = baseDropBonus;
            this.extraDropBonus = extraDropBonus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(challengeId);
            writer.WriteInt(targetId);
            writer.WriteInt(baseXpBonus);
            writer.WriteInt(extraXpBonus);
            writer.WriteInt(baseDropBonus);
            writer.WriteInt(extraDropBonus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            challengeId = reader.ReadSByte();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            targetId = reader.ReadInt();
            baseXpBonus = reader.ReadInt();
            if (baseXpBonus < 0)
                throw new Exception("Forbidden value on baseXpBonus = " + baseXpBonus + ", it doesn't respect the following condition : baseXpBonus < 0");
            extraXpBonus = reader.ReadInt();
            if (extraXpBonus < 0)
                throw new Exception("Forbidden value on extraXpBonus = " + extraXpBonus + ", it doesn't respect the following condition : extraXpBonus < 0");
            baseDropBonus = reader.ReadInt();
            if (baseDropBonus < 0)
                throw new Exception("Forbidden value on baseDropBonus = " + baseDropBonus + ", it doesn't respect the following condition : baseDropBonus < 0");
            extraDropBonus = reader.ReadInt();
            if (extraDropBonus < 0)
                throw new Exception("Forbidden value on extraDropBonus = " + extraDropBonus + ", it doesn't respect the following condition : extraDropBonus < 0");
        }
        
    }
    
}