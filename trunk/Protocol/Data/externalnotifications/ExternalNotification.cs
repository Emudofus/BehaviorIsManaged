

// Generated on 10/25/2012 10:42:59
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ExternalNotifications")]
    public class ExternalNotification : IDataObject
    {
        private const String MODULE = "ExternalNotifications";
        public int id;
        public int categoryId;
        public int iconId;
        public int colorId;
        public uint descriptionId;
        public Boolean defaultEnable;
        public Boolean defaultSound;
        public Boolean defaultMultiAccount;
        public String name;
        public uint messageId;
    }
}