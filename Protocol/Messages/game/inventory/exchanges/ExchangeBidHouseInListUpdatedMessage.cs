

// Generated on 12/11/2012 19:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBidHouseInListUpdatedMessage : ExchangeBidHouseInListAddedMessage
    {
        public const uint Id = 6337;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeBidHouseInListUpdatedMessage()
        {
        }
        
        public ExchangeBidHouseInListUpdatedMessage(int itemUID, int objGenericId, short powerRate, bool overMax, Types.ObjectEffect[] effects, int[] prices)
         : base(itemUID, objGenericId, powerRate, overMax, effects, prices)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}