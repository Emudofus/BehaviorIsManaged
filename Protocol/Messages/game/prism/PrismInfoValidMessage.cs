

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismInfoValidMessage : NetworkMessage
    {
        public const uint Id = 5858;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
        
        public PrismInfoValidMessage()
        {
        }
        
        public PrismInfoValidMessage(Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo)
        {
            this.waitingForHelpInfo = waitingForHelpInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            waitingForHelpInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            waitingForHelpInfo = new Types.ProtectedEntityWaitingForHelpInfo();
            waitingForHelpInfo.Deserialize(reader);
        }
        
    }
    
}