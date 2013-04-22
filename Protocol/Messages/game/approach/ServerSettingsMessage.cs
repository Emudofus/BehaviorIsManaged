

// Generated on 04/17/2013 22:29:37
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
        public sbyte gameType;
        
        public ServerSettingsMessage()
        {
        }
        
        public ServerSettingsMessage(string lang, sbyte community, sbyte gameType)
        {
            this.lang = lang;
            this.community = community;
            this.gameType = gameType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(lang);
            writer.WriteSByte(community);
            writer.WriteSByte(gameType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            lang = reader.ReadUTF();
            community = reader.ReadSByte();
            if (community < 0)
                throw new Exception("Forbidden value on community = " + community + ", it doesn't respect the following condition : community < 0");
            gameType = reader.ReadSByte();
            if (gameType < 0)
                throw new Exception("Forbidden value on gameType = " + gameType + ", it doesn't respect the following condition : gameType < 0");
        }
        
    }
    
}