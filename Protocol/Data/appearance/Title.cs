

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Titles")]
    public class Title : IDataObject
    {
        private const String MODULE = "Titles";
        public int id;
        public uint nameId;
        public Boolean visible;
        public int categoryId;
    }
}