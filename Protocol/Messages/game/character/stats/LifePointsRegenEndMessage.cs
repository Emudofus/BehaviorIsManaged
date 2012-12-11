

// Generated on 12/11/2012 19:44:14
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class LifePointsRegenEndMessage : UpdateLifePointsMessage
    {
        public const uint Id = 5686;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int lifePointsGained;
        
        public LifePointsRegenEndMessage()
        {
        }
        
        public LifePointsRegenEndMessage(int lifePoints, int maxLifePoints, int lifePointsGained)
         : base(lifePoints, maxLifePoints)
        {
            this.lifePointsGained = lifePointsGained;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(lifePointsGained);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            lifePointsGained = reader.ReadInt();
            if (lifePointsGained < 0)
                throw new Exception("Forbidden value on lifePointsGained = " + lifePointsGained + ", it doesn't respect the following condition : lifePointsGained < 0");
        }
        
    }
    
}