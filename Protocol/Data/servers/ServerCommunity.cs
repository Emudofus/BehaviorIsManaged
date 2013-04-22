

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ServerCommunities")]
    public class ServerCommunity : IDataObject
    {
        public const String MODULE = "ServerCommunities";
        public int id;
        public uint nameId;
        public String shortId;
        public List<String> defaultCountries;
    }
}