

// Generated on 09/23/2012 22:27:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SpellListMessage : NetworkMessage
    {
        public const uint Id = 1200;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool spellPrevisualization;
        public Types.SpellItem[] spells;
        
        public SpellListMessage()
        {
        }
        
        public SpellListMessage(bool spellPrevisualization, Types.SpellItem[] spells)
        {
            this.spellPrevisualization = spellPrevisualization;
            this.spells = spells;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(spellPrevisualization);
            writer.WriteUShort((ushort)spells.Length);
            foreach (var entry in spells)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellPrevisualization = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            spells = new Types.SpellItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 spells[i] = new Types.SpellItem();
                 spells[i].Deserialize(reader);
            }
        }
        
    }
    
}