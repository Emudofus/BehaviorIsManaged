

// Generated on 12/11/2012 19:44:38
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