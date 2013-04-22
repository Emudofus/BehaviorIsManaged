

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Url")]
    public class Url : IDataObject
    {
        public const String MODULE = "Url";
        public int id;
        public int browserId;
        public String url;
        public String param;
        public String method;
    }
}