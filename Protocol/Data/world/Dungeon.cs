

// Generated on 12/11/2012 19:44:39
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Dungeons")]
    public class Dungeon : IDataObject
    {
        private const String MODULE = "Dungeons";
        public int id;
        public uint nameId;
        public int optimalPlayerLevel;
    }
}