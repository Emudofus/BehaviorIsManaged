

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ServerPopulations")]
    public class ServerPopulation : IDataObject
    {
        public const String MODULE = "ServerPopulations";
        public int id;
        public uint nameId;
        public int weight;
    }
}