

// Generated on 10/25/2012 10:43:02
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ServerPopulations")]
    public class ServerPopulation : IDataObject
    {
        private const String MODULE = "ServerPopulations";
        public int id;
        public uint nameId;
        public int weight;
    }
}