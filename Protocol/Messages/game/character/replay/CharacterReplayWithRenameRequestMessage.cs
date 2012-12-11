

// Generated on 12/11/2012 19:44:14
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterReplayWithRenameRequestMessage : CharacterReplayRequestMessage
    {
        public const uint Id = 6122;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public CharacterReplayWithRenameRequestMessage()
        {
        }
        
        public CharacterReplayWithRenameRequestMessage(int characterId, string name)
         : base(characterId)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
        }
        
    }
    
}