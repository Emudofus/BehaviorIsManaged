

// Generated on 04/17/2013 22:30:13
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ItemTypes")]
    public class ItemType : IDataObject
    {
        public const String MODULE = "ItemTypes";
        public int id;
        public uint nameId;
        public uint superTypeId;
        public Boolean plural;
        public uint gender;
        public String rawZone;
        public Boolean needUseConfirm;
    }
}