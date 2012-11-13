

// Generated on 10/25/2012 10:42:36
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterReplayWithRecolorRequestMessage : CharacterReplayRequestMessage
    {
        public const uint Id = 6111;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int[] indexedColor;
        
        public CharacterReplayWithRecolorRequestMessage()
        {
        }
        
        public CharacterReplayWithRecolorRequestMessage(int characterId, int[] indexedColor)
         : base(characterId)
        {
            this.indexedColor = indexedColor;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)indexedColor.Length);
            foreach (var entry in indexedColor)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            indexedColor = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 indexedColor[i] = reader.ReadInt();
            }
        }
        
    }
    
}