

// Generated on 04/17/2013 22:29:54
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildInvitationStateRecrutedMessage : NetworkMessage
    {
        public const uint Id = 5548;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte invitationState;
        
        public GuildInvitationStateRecrutedMessage()
        {
        }
        
        public GuildInvitationStateRecrutedMessage(sbyte invitationState)
        {
            this.invitationState = invitationState;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(invitationState);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            invitationState = reader.ReadSByte();
            if (invitationState < 0)
                throw new Exception("Forbidden value on invitationState = " + invitationState + ", it doesn't respect the following condition : invitationState < 0");
        }
        
    }
    
}