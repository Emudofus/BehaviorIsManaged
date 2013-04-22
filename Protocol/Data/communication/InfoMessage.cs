

// Generated on 04/17/2013 22:30:12
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("InfoMessages")]
    public class InfoMessage : IDataObject
    {
        public const String MODULE = "InfoMessages";
        public uint typeId;
        public uint messageId;
        public uint textId;
    }
}