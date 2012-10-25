

// Generated on 10/25/2012 10:43:02
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Servers")]
    public class Server : IDataObject
    {
        private const String MODULE = "Servers";
        public int id;
        public uint nameId;
        public uint commentId;
        public float openingDate;
        public String language;
        public int populationId;
        public uint gameTypeId;
        public int communityId;
        public List<String> restrictedToLanguages;
    }
}