

// Generated on 10/25/2012 10:42:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismAlignmentBonusResultMessage : NetworkMessage
    {
        public const uint Id = 5842;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.AlignmentBonusInformations alignmentBonus;
        
        public PrismAlignmentBonusResultMessage()
        {
        }
        
        public PrismAlignmentBonusResultMessage(Types.AlignmentBonusInformations alignmentBonus)
        {
            this.alignmentBonus = alignmentBonus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            alignmentBonus.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            alignmentBonus = new Types.AlignmentBonusInformations();
            alignmentBonus.Deserialize(reader);
        }
        
    }
    
}