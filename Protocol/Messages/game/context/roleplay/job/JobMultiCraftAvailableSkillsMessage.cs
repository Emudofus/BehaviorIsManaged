

// Generated on 04/17/2013 22:29:47
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class JobMultiCraftAvailableSkillsMessage : JobAllowMultiCraftRequestMessage
    {
        public const uint Id = 5747;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int playerId;
        public short[] skills;
        
        public JobMultiCraftAvailableSkillsMessage()
        {
        }
        
        public JobMultiCraftAvailableSkillsMessage(bool enabled, int playerId, short[] skills)
         : base(enabled)
        {
            this.playerId = playerId;
            this.skills = skills;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(playerId);
            writer.WriteUShort((ushort)skills.Length);
            foreach (var entry in skills)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            var limit = reader.ReadUShort();
            skills = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 skills[i] = reader.ReadShort();
            }
        }
        
    }
    
}