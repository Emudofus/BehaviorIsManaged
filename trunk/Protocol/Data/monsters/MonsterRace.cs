

// Generated on 09/23/2012 21:40:24
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("MonsterRace")]
    public class MonsterRace : IDataObject
    {
        private const String MODULE = "MonsterRaces";
        public int id;
        public int superRaceId;
        public uint nameId;
    }
}