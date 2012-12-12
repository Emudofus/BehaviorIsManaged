

// Generated on 12/11/2012 19:44:14
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class UpdateLifePointsMessage : NetworkMessage
    {
        public const uint Id = 5658;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int lifePoints;
        public int maxLifePoints;
        
        public UpdateLifePointsMessage()
        {
        }
        
        public UpdateLifePointsMessage(int lifePoints, int maxLifePoints)
        {
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(lifePoints);
            writer.WriteInt(maxLifePoints);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            lifePoints = reader.ReadInt();
            if (lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            maxLifePoints = reader.ReadInt();
            if (maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
        }
        
    }
    
}