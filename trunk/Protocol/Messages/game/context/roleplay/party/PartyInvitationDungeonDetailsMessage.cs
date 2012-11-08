#region License GNU GPL
// PartyInvitationDungeonDetailsMessage.cs
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
    public class PartyInvitationDungeonDetailsMessage : PartyInvitationDetailsMessage
    {
        public const uint Id = 6262;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        public bool[] playersDungeonReady;
        
        public PartyInvitationDungeonDetailsMessage()
        {
        }
        
        public PartyInvitationDungeonDetailsMessage(int partyId, sbyte partyType, int fromId, string fromName, int leaderId, Types.PartyInvitationMemberInformations[] members, Types.PartyGuestInformations[] guests, short dungeonId, bool[] playersDungeonReady)
         : base(partyId, partyType, fromId, fromName, leaderId, members, guests)
        {
            this.dungeonId = dungeonId;
            this.playersDungeonReady = playersDungeonReady;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(dungeonId);
            writer.WriteUShort((ushort)playersDungeonReady.Length);
            foreach (var entry in playersDungeonReady)
            {
                 writer.WriteBoolean(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            dungeonId = reader.ReadShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadUShort();
            playersDungeonReady = new bool[limit];
            for (int i = 0; i < limit; i++)
            {
                 playersDungeonReady[i] = reader.ReadBoolean();
            }
        }
        
    }
    
}