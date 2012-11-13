

// Generated on 10/25/2012 10:42:52
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SetUpdateMessage : NetworkMessage
    {
        public const uint Id = 5503;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short setId;
        public short[] setObjects;
        public Types.ObjectEffect[] setEffects;
        
        public SetUpdateMessage()
        {
        }
        
        public SetUpdateMessage(short setId, short[] setObjects, Types.ObjectEffect[] setEffects)
        {
            this.setId = setId;
            this.setObjects = setObjects;
            this.setEffects = setEffects;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(setId);
            writer.WriteUShort((ushort)setObjects.Length);
            foreach (var entry in setObjects)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)setEffects.Length);
            foreach (var entry in setEffects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            setId = reader.ReadShort();
            if (setId < 0)
                throw new Exception("Forbidden value on setId = " + setId + ", it doesn't respect the following condition : setId < 0");
            var limit = reader.ReadUShort();
            setObjects = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 setObjects[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            setEffects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 setEffects[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 setEffects[i].Deserialize(reader);
            }
        }
        
    }
    
}