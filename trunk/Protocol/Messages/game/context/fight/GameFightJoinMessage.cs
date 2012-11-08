#region License GNU GPL
// GameFightJoinMessage.cs
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
    public class GameFightJoinMessage : NetworkMessage
    {
        public const uint Id = 702;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool canBeCancelled;
        public bool canSayReady;
        public bool isSpectator;
        public bool isFightStarted;
        public int timeMaxBeforeFightStart;
        public sbyte fightType;
        
        public GameFightJoinMessage()
        {
        }
        
        public GameFightJoinMessage(bool canBeCancelled, bool canSayReady, bool isSpectator, bool isFightStarted, int timeMaxBeforeFightStart, sbyte fightType)
        {
            this.canBeCancelled = canBeCancelled;
            this.canSayReady = canSayReady;
            this.isSpectator = isSpectator;
            this.isFightStarted = isFightStarted;
            this.timeMaxBeforeFightStart = timeMaxBeforeFightStart;
            this.fightType = fightType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, canBeCancelled);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, canSayReady);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, isSpectator);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, isFightStarted);
            writer.WriteByte(flag1);
            writer.WriteInt(timeMaxBeforeFightStart);
            writer.WriteSByte(fightType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            canBeCancelled = BooleanByteWrapper.GetFlag(flag1, 0);
            canSayReady = BooleanByteWrapper.GetFlag(flag1, 1);
            isSpectator = BooleanByteWrapper.GetFlag(flag1, 2);
            isFightStarted = BooleanByteWrapper.GetFlag(flag1, 3);
            timeMaxBeforeFightStart = reader.ReadInt();
            if (timeMaxBeforeFightStart < 0)
                throw new Exception("Forbidden value on timeMaxBeforeFightStart = " + timeMaxBeforeFightStart + ", it doesn't respect the following condition : timeMaxBeforeFightStart < 0");
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
        }
        
    }
    
}