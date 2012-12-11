

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Heads")]
    public class Head : IDataObject
    {
        private const String MODULE = "Heads";
        public int id;
        public String skins;
        public String assetId;
        public uint breed;
        public uint gender;
        public uint order;
    }
}