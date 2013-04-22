

// Generated on 04/17/2013 22:29:46
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AccountHouseMessage : NetworkMessage
    {
        public const uint Id = 6315;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.AccountHouseInformations[] houses;
        
        public AccountHouseMessage()
        {
        }
        
        public AccountHouseMessage(Types.AccountHouseInformations[] houses)
        {
            this.houses = houses;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)houses.Length);
            foreach (var entry in houses)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            houses = new Types.AccountHouseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 houses[i] = new Types.AccountHouseInformations();
                 houses[i].Deserialize(reader);
            }
        }
        
    }
    
}