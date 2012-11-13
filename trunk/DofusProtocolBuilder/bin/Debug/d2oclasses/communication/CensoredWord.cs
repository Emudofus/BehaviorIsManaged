

// Generated on 10/25/2012 10:42:59
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("CensoredWords")]
    public class CensoredWord : IDataObject
    {
        private const String MODULE = "CensoredWords";
        public uint id;
        public uint listId;
        public String language;
        public String word;
        public Boolean deepLooking;
    }
}