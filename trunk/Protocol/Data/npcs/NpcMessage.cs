

// Generated on 09/23/2012 21:40:24
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("NpcMessage")]
    public class NpcMessage : IDataObject
    {
        private const String MODULE = "NpcMessages";
        public int id;
        public uint messageId;
        public String messageParams;
    }
}