

// Generated on 12/11/2012 19:44:21
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