

// Generated on 12/11/2012 19:44:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightTeamMemberTaxCollectorInformations : FightTeamMemberInformations
    {
        public const short Id = 177;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short firstNameId;
        public short lastNameId;
        public byte level;
        public int guildId;
        public int uid;
        
        public FightTeamMemberTaxCollectorInformations()
        {
        }
        
        public FightTeamMemberTaxCollectorInformations(int id, short firstNameId, short lastNameId, byte level, int guildId, int uid)
         : base(id)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.level = level;
            this.guildId = guildId;
            this.uid = uid;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(firstNameId);
            writer.WriteShort(lastNameId);
            writer.WriteByte(level);
            writer.WriteInt(guildId);
            writer.WriteInt(uid);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            firstNameId = reader.ReadShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            level = reader.ReadByte();
            if (level < 1 || level > 200)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 1 || level > 200");
            guildId = reader.ReadInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            uid = reader.ReadInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
        }
        
    }
    
}