

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ShortcutBarContentMessage : NetworkMessage
    {
        public const uint Id = 6231;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public Types.Shortcut[] shortcuts;
        
        public ShortcutBarContentMessage()
        {
        }
        
        public ShortcutBarContentMessage(sbyte barType, Types.Shortcut[] shortcuts)
        {
            this.barType = barType;
            this.shortcuts = shortcuts;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            writer.WriteUShort((ushort)shortcuts.Length);
            foreach (var entry in shortcuts)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            var limit = reader.ReadUShort();
            shortcuts = new Types.Shortcut[limit];
            for (int i = 0; i < limit; i++)
            {
                 shortcuts[i] = Types.ProtocolTypeManager.GetInstance<Types.Shortcut>(reader.ReadShort());
                 shortcuts[i].Deserialize(reader);
            }
        }
        
    }
    
}