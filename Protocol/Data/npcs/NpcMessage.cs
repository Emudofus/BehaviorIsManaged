

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("NpcMessages")]
    public class NpcMessage : IDataObject
    {
        private const String MODULE = "NpcMessages";
        public int id;
        public uint messageId;
        public String messageParams;
    }
}