

// Generated on 12/11/2012 19:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharactersListMessage : NetworkMessage
    {
        public const uint Id = 151;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool hasStartupActions;
        public Types.CharacterBaseInformations[] characters;
        
        public CharactersListMessage()
        {
        }
        
        public CharactersListMessage(bool hasStartupActions, Types.CharacterBaseInformations[] characters)
        {
            this.hasStartupActions = hasStartupActions;
            this.characters = characters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(hasStartupActions);
            writer.WriteUShort((ushort)characters.Length);
            foreach (var entry in characters)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hasStartupActions = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            characters = new Types.CharacterBaseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 characters[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterBaseInformations>(reader.ReadShort());
                 characters[i].Deserialize(reader);
            }
        }
        
    }
    
}