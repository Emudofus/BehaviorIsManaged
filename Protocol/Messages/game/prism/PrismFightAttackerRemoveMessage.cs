

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismFightAttackerRemoveMessage : NetworkMessage
    {
        public const uint Id = 5897;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public int fighterToRemoveId;
        
        public PrismFightAttackerRemoveMessage()
        {
        }
        
        public PrismFightAttackerRemoveMessage(double fightId, int fighterToRemoveId)
        {
            this.fightId = fightId;
            this.fighterToRemoveId = fighterToRemoveId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            writer.WriteInt(fighterToRemoveId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            fighterToRemoveId = reader.ReadInt();
            if (fighterToRemoveId < 0)
                throw new Exception("Forbidden value on fighterToRemoveId = " + fighterToRemoveId + ", it doesn't respect the following condition : fighterToRemoveId < 0");
        }
        
    }
    
}