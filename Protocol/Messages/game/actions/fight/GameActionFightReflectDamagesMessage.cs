

// Generated on 12/11/2012 19:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightReflectDamagesMessage : AbstractGameActionMessage
    {
        public const uint Id = 5530;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public int amount;
        
        public GameActionFightReflectDamagesMessage()
        {
        }
        
        public GameActionFightReflectDamagesMessage(short actionId, int sourceId, int targetId, int amount)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.amount = amount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteInt(amount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            amount = reader.ReadInt();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
        }
        
    }
    
}