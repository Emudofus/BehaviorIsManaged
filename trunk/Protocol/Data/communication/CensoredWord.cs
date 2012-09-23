

// Generated on 09/23/2012 21:40:19
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("CensoredWord")]
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