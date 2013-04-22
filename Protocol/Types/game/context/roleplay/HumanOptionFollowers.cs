

// Generated on 04/17/2013 22:30:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HumanOptionFollowers : HumanOption
    {
        public const short Id = 410;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.IndexedEntityLook[] followingCharactersLook;
        
        public HumanOptionFollowers()
        {
        }
        
        public HumanOptionFollowers(Types.IndexedEntityLook[] followingCharactersLook)
        {
            this.followingCharactersLook = followingCharactersLook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)followingCharactersLook.Length);
            foreach (var entry in followingCharactersLook)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            followingCharactersLook = new Types.IndexedEntityLook[limit];
            for (int i = 0; i < limit; i++)
            {
                 followingCharactersLook[i] = new Types.IndexedEntityLook();
                 followingCharactersLook[i].Deserialize(reader);
            }
        }
        
    }
    
}