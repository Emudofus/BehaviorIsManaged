

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightShowFighterRandomStaticPoseMessage : GameFightShowFighterMessage
    {
        public const uint Id = 6218;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameFightShowFighterRandomStaticPoseMessage()
        {
        }
        
        public GameFightShowFighterRandomStaticPoseMessage(Types.GameFightFighterInformations informations)
         : base(informations)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}