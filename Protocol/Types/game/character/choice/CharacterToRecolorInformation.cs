

// Generated on 04/17/2013 22:30:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class CharacterToRecolorInformation : AbstractCharacterInformation
    {
        public const short Id = 212;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int[] colors;
        
        public CharacterToRecolorInformation()
        {
        }
        
        public CharacterToRecolorInformation(int id, int[] colors)
         : base(id)
        {
            this.colors = colors;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)colors.Length);
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            colors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 colors[i] = reader.ReadInt();
            }
        }
        
    }
    
}