

// Generated on 04/17/2013 22:29:46
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameRolePlayShowChallengeMessage : NetworkMessage
    {
        public const uint Id = 301;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FightCommonInformations commonsInfos;
        
        public GameRolePlayShowChallengeMessage()
        {
        }
        
        public GameRolePlayShowChallengeMessage(Types.FightCommonInformations commonsInfos)
        {
            this.commonsInfos = commonsInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            commonsInfos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            commonsInfos = new Types.FightCommonInformations();
            commonsInfos.Deserialize(reader);
        }
        
    }
    
}