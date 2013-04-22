

// Generated on 04/17/2013 22:30:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class IgnoredInformations : AbstractContactInformations
    {
        public const short Id = 106;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public IgnoredInformations()
        {
        }
        
        public IgnoredInformations(int accountId, string accountName)
         : base(accountId, accountName)
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