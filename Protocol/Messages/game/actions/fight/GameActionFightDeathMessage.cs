

// Generated on 04/17/2013 22:29:35
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightDeathMessage : AbstractGameActionMessage
    {
        public const uint Id = 1099;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        
        public GameActionFightDeathMessage()
        {
        }
        
        public GameActionFightDeathMessage(short actionId, int sourceId, int targetId)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
        }
        
    }
    
}