

// Generated on 09/23/2012 22:26:54
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyInvitationDetailsMessage : AbstractPartyMessage
    {
        public const uint Id = 6263;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte partyType;
        public int fromId;
        public string fromName;
        public int leaderId;
        public Types.PartyInvitationMemberInformations[] members;
        public Types.PartyGuestInformations[] guests;
        
        public PartyInvitationDetailsMessage()
        {
        }
        
        public PartyInvitationDetailsMessage(int partyId, sbyte partyType, int fromId, string fromName, int leaderId, Types.PartyInvitationMemberInformations[] members, Types.PartyGuestInformations[] guests)
         : base(partyId)
        {
            this.partyType = partyType;
            this.fromId = fromId;
            this.fromName = fromName;
            this.leaderId = leaderId;
            this.members = members;
            this.guests = guests;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(partyType);
            writer.WriteInt(fromId);
            writer.WriteUTF(fromName);
            writer.WriteInt(leaderId);
            writer.WriteUShort((ushort)members.Length);
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)guests.Length);
            foreach (var entry in guests)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyType = reader.ReadSByte();
            if (partyType < 0)
                throw new Exception("Forbidden value on partyType = " + partyType + ", it doesn't respect the following condition : partyType < 0");
            fromId = reader.ReadInt();
            if (fromId < 0)
                throw new Exception("Forbidden value on fromId = " + fromId + ", it doesn't respect the following condition : fromId < 0");
            fromName = reader.ReadUTF();
            leaderId = reader.ReadInt();
            if (leaderId < 0)
                throw new Exception("Forbidden value on leaderId = " + leaderId + ", it doesn't respect the following condition : leaderId < 0");
            var limit = reader.ReadUShort();
            members = new Types.PartyInvitationMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 members[i] = new Types.PartyInvitationMemberInformations();
                 members[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            guests = new Types.PartyGuestInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 guests[i] = new Types.PartyGuestInformations();
                 guests[i].Deserialize(reader);
            }
        }
        
    }
    
}