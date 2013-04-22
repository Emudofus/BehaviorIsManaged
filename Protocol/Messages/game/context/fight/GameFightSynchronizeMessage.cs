

// Generated on 04/17/2013 22:29:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightSynchronizeMessage : NetworkMessage
    {
        public const uint Id = 5921;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameFightFighterInformations[] fighters;
        
        public GameFightSynchronizeMessage()
        {
        }
        
        public GameFightSynchronizeMessage(Types.GameFightFighterInformations[] fighters)
        {
            this.fighters = fighters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)fighters.Length);
            foreach (var entry in fighters)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            fighters = new Types.GameFightFighterInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fighters[i] = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterInformations>(reader.ReadShort());
                 fighters[i].Deserialize(reader);
            }
        }
        
    }
    
}