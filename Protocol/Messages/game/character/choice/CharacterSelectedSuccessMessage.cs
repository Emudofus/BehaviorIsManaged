

// Generated on 04/17/2013 22:29:38
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterSelectedSuccessMessage : NetworkMessage
    {
        public const uint Id = 153;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.CharacterBaseInformations infos;
        
        public CharacterSelectedSuccessMessage()
        {
        }
        
        public CharacterSelectedSuccessMessage(Types.CharacterBaseInformations infos)
        {
            this.infos = infos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            infos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            infos = new Types.CharacterBaseInformations();
            infos.Deserialize(reader);
        }
        
    }
    
}