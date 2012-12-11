

// Generated on 12/11/2012 19:44:39
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