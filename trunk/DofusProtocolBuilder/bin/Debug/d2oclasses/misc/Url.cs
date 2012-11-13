

// Generated on 10/25/2012 10:43:01
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Url")]
    public class Url : IDataObject
    {
        private const String MODULE = "Url";
        public int id;
        public int browserId;
        public String url;
        public String param;
        public String method;
    }
}