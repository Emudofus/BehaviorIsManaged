

// Generated on 04/17/2013 22:29:36
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightModifyEffectsDurationMessage : AbstractGameActionMessage
    {
        public const uint Id = 6304;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short delta;
        
        public GameActionFightModifyEffectsDurationMessage()
        {
        }
        
        public GameActionFightModifyEffectsDurationMessage(short actionId, int sourceId, int targetId, short delta)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.delta = delta;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(delta);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            delta = reader.ReadShort();
        }
        
    }
    
}