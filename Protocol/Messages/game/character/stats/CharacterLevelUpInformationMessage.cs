

// Generated on 04/17/2013 22:29:39
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterLevelUpInformationMessage : CharacterLevelUpMessage
    {
        public const uint Id = 6076;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public int id;
        
        public CharacterLevelUpInformationMessage()
        {
        }
        
        public CharacterLevelUpInformationMessage(byte newLevel, string name, int id)
         : base(newLevel)
        {
            this.name = name;
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
            writer.WriteInt(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
        }
        
    }
    
}