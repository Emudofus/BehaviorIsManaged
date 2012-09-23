

// Generated on 09/23/2012 22:26:45
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightLifePointsGainMessage : AbstractGameActionMessage
    {
        public const uint Id = 6311;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short delta;
        
        public GameActionFightLifePointsGainMessage()
        {
        }
        
        public GameActionFightLifePointsGainMessage(short actionId, int sourceId, int targetId, short delta)
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
            if (delta < 0)
                throw new Exception("Forbidden value on delta = " + delta + ", it doesn't respect the following condition : delta < 0");
        }
        
    }
    
}