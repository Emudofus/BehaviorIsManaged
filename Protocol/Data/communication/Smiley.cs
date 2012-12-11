

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Smileys")]
    public class Smiley : IDataObject
    {
        private const String MODULE = "Smileys";
        public uint id;
        public uint order;
        public String gfxId;
        public Boolean forPlayers;
        public List<String> triggers;
    }
}