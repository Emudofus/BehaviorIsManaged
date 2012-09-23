

// Generated on 09/23/2012 21:40:25
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ServerPopulation")]
    public class ServerPopulation : IDataObject
    {
        private const String MODULE = "ServerPopulations";
        public int id;
        public uint nameId;
        public int weight;
    }
}