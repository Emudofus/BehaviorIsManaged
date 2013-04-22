

// Generated on 04/17/2013 22:29:39
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterCreationRequestMessage : NetworkMessage
    {
        public const uint Id = 160;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public sbyte breed;
        public bool sex;
        public int[] colors;
        public int cosmeticId;
        
        public CharacterCreationRequestMessage()
        {
        }
        
        public CharacterCreationRequestMessage(string name, sbyte breed, bool sex, int[] colors, int cosmeticId)
        {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.colors = colors;
            this.cosmeticId = cosmeticId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteInt(cosmeticId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            breed = reader.ReadSByte();
            if (breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Steamer)
                throw new Exception("Forbidden value on breed = " + breed + ", it doesn't respect the following condition : breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Steamer");
            sex = reader.ReadBoolean();
            colors = new int[5];
            for (int i = 0; i < 5; i++)
            {
                 colors[i] = reader.ReadInt();
            }
            cosmeticId = reader.ReadInt();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
        }
        
    }
    
}