

// Generated on 04/17/2013 22:30:02
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismFightDefenderLeaveMessage : NetworkMessage
    {
        public const uint Id = 5892;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public int fighterToRemoveId;
        public int successor;
        
        public PrismFightDefenderLeaveMessage()
        {
        }
        
        public PrismFightDefenderLeaveMessage(double fightId, int fighterToRemoveId, int successor)
        {
            this.fightId = fightId;
            this.fighterToRemoveId = fighterToRemoveId;
            this.successor = successor;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            writer.WriteInt(fighterToRemoveId);
            writer.WriteInt(successor);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            fighterToRemoveId = reader.ReadInt();
            if (fighterToRemoveId < 0)
                throw new Exception("Forbidden value on fighterToRemoveId = " + fighterToRemoveId + ", it doesn't respect the following condition : fighterToRemoveId < 0");
            successor = reader.ReadInt();
            if (successor < 0)
                throw new Exception("Forbidden value on successor = " + successor + ", it doesn't respect the following condition : successor < 0");
        }
        
    }
    
}