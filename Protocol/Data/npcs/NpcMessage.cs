

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("NpcMessages")]
    public class NpcMessage : IDataObject
    {
        public const String MODULE = "NpcMessages";
        public int id;
        public uint messageId;
        public String messageParams;
    }
}