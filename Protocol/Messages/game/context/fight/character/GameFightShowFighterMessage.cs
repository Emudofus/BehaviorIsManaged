

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightShowFighterMessage : NetworkMessage
    {
        public const uint Id = 5864;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameFightFighterInformations informations;
        
        public GameFightShowFighterMessage()
        {
        }
        
        public GameFightShowFighterMessage(Types.GameFightFighterInformations informations)
        {
            this.informations = informations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(informations.TypeId);
            informations.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            informations = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterInformations>(reader.ReadShort());
            informations.Deserialize(reader);
        }
        
    }
    
}