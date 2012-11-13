

// Generated on 10/25/2012 10:43:01
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("MonsterRaces")]
    public class MonsterRace : IDataObject
    {
        private const String MODULE = "MonsterRaces";
        public int id;
        public int superRaceId;
        public uint nameId;
    }
}