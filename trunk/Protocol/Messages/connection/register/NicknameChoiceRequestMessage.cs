

// Generated on 09/23/2012 22:26:44
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class NicknameChoiceRequestMessage : NetworkMessage
    {
        public const uint Id = 5639;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string nickname;
        
        public NicknameChoiceRequestMessage()
        {
        }
        
        public NicknameChoiceRequestMessage(string nickname)
        {
            this.nickname = nickname;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(nickname);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            nickname = reader.ReadUTF();
        }
        
    }
    
}