

// Generated on 09/23/2012 22:27:11
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ShortcutObject : Shortcut
    {
        public const short Id = 367;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public ShortcutObject()
        {
        }
        
        public ShortcutObject(int slot)
         : base(slot)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}