

// Generated on 04/17/2013 22:29:34
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionAcknowledgementMessage : NetworkMessage
    {
        public const uint Id = 957;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool valid;
        public sbyte actionId;
        
        public GameActionAcknowledgementMessage()
        {
        }
        
        public GameActionAcknowledgementMessage(bool valid, sbyte actionId)
        {
            this.valid = valid;
            this.actionId = actionId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(valid);
            writer.WriteSByte(actionId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            valid = reader.ReadBoolean();
            actionId = reader.ReadSByte();
        }
        
    }
    
}