

// Generated on 09/23/2012 22:27:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeCraftResultMagicWithObjectDescMessage : ExchangeCraftResultWithObjectDescMessage
    {
        public const uint Id = 6188;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte magicPoolStatus;
        
        public ExchangeCraftResultMagicWithObjectDescMessage()
        {
        }
        
        public ExchangeCraftResultMagicWithObjectDescMessage(sbyte craftResult, Types.ObjectItemNotInContainer objectInfo, sbyte magicPoolStatus)
         : base(craftResult, objectInfo)
        {
            this.magicPoolStatus = magicPoolStatus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(magicPoolStatus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            magicPoolStatus = reader.ReadSByte();
        }
        
    }
    
}