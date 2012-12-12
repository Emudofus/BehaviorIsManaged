

// Generated on 12/11/2012 19:44:34
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class DungeonPartyFinderPlayer
    {
        public const short Id = 373;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int playerId;
        public string playerName;
        public sbyte breed;
        public bool sex;
        public short level;
        
        public DungeonPartyFinderPlayer()
        {
        }
        
        public DungeonPartyFinderPlayer(int playerId, string playerName, sbyte breed, bool sex, short level)
        {
            this.playerId = playerId;
            this.playerName = playerName;
            this.breed = breed;
            this.sex = sex;
            this.level = level;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(playerId);
            writer.WriteUTF(playerName);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            writer.WriteShort(level);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            playerName = reader.ReadUTF();
            breed = reader.ReadSByte();
            if (breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Steamer)
                throw new Exception("Forbidden value on breed = " + breed + ", it doesn't respect the following condition : breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Steamer");
            sex = reader.ReadBoolean();
            level = reader.ReadShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
        }
        
    }
    
}