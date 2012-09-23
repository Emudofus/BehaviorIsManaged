

// Generated on 09/23/2012 21:40:24
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Pack")]
    public class Pack : IDataObject
    {
        private const String MODULE = "Pack";
        public int id;
        public String name;
        public Boolean hasSubAreas;
    }
}