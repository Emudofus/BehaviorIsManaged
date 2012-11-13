

// Generated on 10/25/2012 10:43:01
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("CensoredContents")]
    public class CensoredContent : IDataObject
    {
        public const String MODULE = "CensoredContents";
        public int type;
        public int oldValue;
        public int newValue;
        public String lang;
    }
}