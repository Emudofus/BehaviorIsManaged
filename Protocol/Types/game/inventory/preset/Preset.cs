

// Generated on 12/11/2012 19:44:35
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class Preset
    {
        public const short Id = 355;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte symbolId;
        public bool mount;
        public Types.PresetItem[] objects;
        
        public Preset()
        {
        }
        
        public Preset(sbyte presetId, sbyte symbolId, bool mount, Types.PresetItem[] objects)
        {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.mount = mount;
            this.objects = objects;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(symbolId);
            writer.WriteBoolean(mount);
            writer.WriteUShort((ushort)objects.Length);
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            symbolId = reader.ReadSByte();
            if (symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + symbolId + ", it doesn't respect the following condition : symbolId < 0");
            mount = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            objects = new Types.PresetItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objects[i] = new Types.PresetItem();
                 objects[i].Deserialize(reader);
            }
        }
        
    }
    
}