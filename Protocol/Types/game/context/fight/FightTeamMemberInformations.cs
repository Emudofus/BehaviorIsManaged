

// Generated on 12/11/2012 19:44:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightTeamMemberInformations
    {
        public const short Id = 44;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int id;
        
        public FightTeamMemberInformations()
        {
        }
        
        public FightTeamMemberInformations(int id)
        {
            this.id = id;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
        }
        
    }
    
}