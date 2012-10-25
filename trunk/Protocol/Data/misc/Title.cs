

// Generated on 10/25/2012 10:43:01
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
        public String color;
    }
}