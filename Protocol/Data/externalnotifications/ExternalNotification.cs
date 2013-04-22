

// Generated on 04/17/2013 22:30:12
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ExternalNotifications")]
    public class ExternalNotification : IDataObject
    {
        public const String MODULE = "ExternalNotifications";
        public int id;
        public int categoryId;
        public int iconId;
        public int colorId;
        public uint descriptionId;
        public Boolean defaultEnable;
        public Boolean defaultSound;
        public Boolean defaultNotify;
        public Boolean defaultMultiAccount;
        public String name;
        public uint messageId;
    }
}