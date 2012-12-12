

// Generated on 12/11/2012 19:44:20
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class EntityTalkMessage : NetworkMessage
    {
        public const uint Id = 6110;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int entityId;
        public short textId;
        public string[] parameters;
        
        public EntityTalkMessage()
        {
        }
        
        public EntityTalkMessage(int entityId, short textId, string[] parameters)
        {
            this.entityId = entityId;
            this.textId = textId;
            this.parameters = parameters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteShort(textId);
            writer.WriteUShort((ushort)parameters.Length);
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            entityId = reader.ReadInt();
            textId = reader.ReadShort();
            if (textId < 0)
                throw new Exception("Forbidden value on textId = " + textId + ", it doesn't respect the following condition : textId < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 parameters[i] = reader.ReadUTF();
            }
        }
        
    }
    
}