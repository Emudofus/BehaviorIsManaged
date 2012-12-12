

// Generated on 12/11/2012 19:44:38
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