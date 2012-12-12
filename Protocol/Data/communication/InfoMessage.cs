

// Generated on 12/11/2012 19:44:36
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