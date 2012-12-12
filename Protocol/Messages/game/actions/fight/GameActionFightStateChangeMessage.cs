

// Generated on 12/11/2012 19:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightStateChangeMessage : AbstractGameActionMessage
    {
        public const uint Id = 5569;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short stateId;
        public bool active;
        
        public GameActionFightStateChangeMessage()
        {
        }
        
        public GameActionFightStateChangeMessage(short actionId, int sourceId, int targetId, short stateId, bool active)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.stateId = stateId;
            this.active = active;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(stateId);
            writer.WriteBoolean(active);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            stateId = reader.ReadShort();
            active = reader.ReadBoolean();
        }
        
    }
    
}