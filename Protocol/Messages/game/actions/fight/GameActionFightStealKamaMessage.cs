

// Generated on 04/17/2013 22:29:36
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightStealKamaMessage : AbstractGameActionMessage
    {
        public const uint Id = 5535;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short amount;
        
        public GameActionFightStealKamaMessage()
        {
        }
        
        public GameActionFightStealKamaMessage(short actionId, int sourceId, int targetId, short amount)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.amount = amount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(amount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            amount = reader.ReadShort();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
        }
        
    }
    
}