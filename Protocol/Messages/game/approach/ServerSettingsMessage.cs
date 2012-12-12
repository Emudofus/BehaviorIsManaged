

// Generated on 12/11/2012 19:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ServerSettingsMessage : NetworkMessage
    {
        public const uint Id = 6340;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string lang;
        public sbyte community;
        
        public ServerSettingsMessage()
        {
        }
        
        public ServerSettingsMessage(string lang, sbyte community)
        {
            this.lang = lang;
            this.community = community;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(lang);
            writer.WriteSByte(community);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            lang = reader.ReadUTF();
            community = reader.ReadSByte();
            if (community < 0)
                throw new Exception("Forbidden value on community = " + community + ", it doesn't respect the following condition : community < 0");
        }
        
    }
    
}