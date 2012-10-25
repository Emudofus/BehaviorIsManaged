

// Generated on 10/25/2012 10:42:59
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("InfoMessages")]
    public class InfoMessage : IDataObject
    {
        private const String MODULE = "InfoMessages";
        public uint typeId;
        public uint messageId;
        public uint textId;
    }
}