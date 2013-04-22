

// Generated on 04/17/2013 22:29:34
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightChangeLookMessage : AbstractGameActionMessage
    {
        public const uint Id = 5532;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public Types.EntityLook entityLook;
        
        public GameActionFightChangeLookMessage()
        {
        }
        
        public GameActionFightChangeLookMessage(short actionId, int sourceId, int targetId, Types.EntityLook entityLook)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.entityLook = entityLook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            entityLook.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            entityLook = new Types.EntityLook();
            entityLook.Deserialize(reader);
        }
        
    }
    
}