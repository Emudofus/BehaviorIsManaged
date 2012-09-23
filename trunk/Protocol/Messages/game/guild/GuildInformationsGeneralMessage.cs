

// Generated on 09/23/2012 22:26:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildInformationsGeneralMessage : NetworkMessage
    {
        public const uint Id = 5557;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enabled;
        public bool abandonnedPaddock;
        public byte level;
        public double expLevelFloor;
        public double experience;
        public double expNextLevelFloor;
        public int creationDate;
        
        public GuildInformationsGeneralMessage()
        {
        }
        
        public GuildInformationsGeneralMessage(bool enabled, bool abandonnedPaddock, byte level, double expLevelFloor, double experience, double expNextLevelFloor, int creationDate)
        {
            this.enabled = enabled;
            this.abandonnedPaddock = abandonnedPaddock;
            this.level = level;
            this.expLevelFloor = expLevelFloor;
            this.experience = experience;
            this.expNextLevelFloor = expNextLevelFloor;
            this.creationDate = creationDate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, enabled);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, abandonnedPaddock);
            writer.WriteByte(flag1);
            writer.WriteByte(level);
            writer.WriteDouble(expLevelFloor);
            writer.WriteDouble(experience);
            writer.WriteDouble(expNextLevelFloor);
            writer.WriteInt(creationDate);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            enabled = BooleanByteWrapper.GetFlag(flag1, 0);
            abandonnedPaddock = BooleanByteWrapper.GetFlag(flag1, 1);
            level = reader.ReadByte();
            if (level < 0 || level > 255)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0 || level > 255");
            expLevelFloor = reader.ReadDouble();
            if (expLevelFloor < 0)
                throw new Exception("Forbidden value on expLevelFloor = " + expLevelFloor + ", it doesn't respect the following condition : expLevelFloor < 0");
            experience = reader.ReadDouble();
            if (experience < 0)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0");
            expNextLevelFloor = reader.ReadDouble();
            if (expNextLevelFloor < 0)
                throw new Exception("Forbidden value on expNextLevelFloor = " + expNextLevelFloor + ", it doesn't respect the following condition : expNextLevelFloor < 0");
            creationDate = reader.ReadInt();
            if (creationDate < 0)
                throw new Exception("Forbidden value on creationDate = " + creationDate + ", it doesn't respect the following condition : creationDate < 0");
        }
        
    }
    
}