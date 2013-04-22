

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Pack")]
    public class Pack : IDataObject
    {
        public const String MODULE = "Pack";
        public int id;
        public String name;
        public Boolean hasSubAreas;
    }
}