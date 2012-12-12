

// Generated on 12/11/2012 19:44:38
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