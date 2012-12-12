

// Generated on 12/11/2012 19:44:20
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TaxCollectorDialogQuestionBasicMessage : NetworkMessage
    {
        public const uint Id = 5619;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.BasicGuildInformations guildInfo;
        
        public TaxCollectorDialogQuestionBasicMessage()
        {
        }
        
        public TaxCollectorDialogQuestionBasicMessage(Types.BasicGuildInformations guildInfo)
        {
            this.guildInfo = guildInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            guildInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guildInfo = new Types.BasicGuildInformations();
            guildInfo.Deserialize(reader);
        }
        
    }
    
}