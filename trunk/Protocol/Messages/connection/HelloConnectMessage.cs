

// Generated on 09/23/2012 22:26:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HelloConnectMessage : NetworkMessage
    {
        public const uint Id = 3;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string salt;
        public sbyte[] key;
        
        public HelloConnectMessage()
        {
        }
        
        public HelloConnectMessage(string salt, sbyte[] key)
        {
            this.salt = salt;
            this.key = key;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(salt);
            writer.WriteUShort((ushort)key.Length);
            foreach (var entry in key)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            salt = reader.ReadUTF();
            var limit = reader.ReadUShort();
            key = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 key[i] = reader.ReadSByte();
            }
        }
        
    }
    
}