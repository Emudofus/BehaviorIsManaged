

// Generated on 04/17/2013 22:29:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightRefreshFighterMessage : NetworkMessage
    {
        public const uint Id = 6309;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameContextActorInformations informations;
        
        public GameFightRefreshFighterMessage()
        {
        }
        
        public GameFightRefreshFighterMessage(Types.GameContextActorInformations informations)
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
            informations = Types.ProtocolTypeManager.GetInstance<Types.GameContextActorInformations>(reader.ReadShort());
            informations.Deserialize(reader);
        }
        
    }
    
}