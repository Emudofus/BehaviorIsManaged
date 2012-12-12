

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Notifications")]
    public class Notification : IDataObject
    {
        private const String MODULE = "Notifications";
        public int id;
        public uint titleId;
        public uint messageId;
        public int iconId;
        public int typeId;
        public String trigger;
    }
}