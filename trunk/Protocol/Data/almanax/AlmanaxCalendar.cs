

// Generated on 10/25/2012 10:42:59
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlmanaxCalendars")]
    public class AlmanaxCalendar : IDataObject
    {
        private const String MODULE = "AlmanaxCalendars";
        public int id;
        public uint nameId;
        public uint descId;
        public int npcId;
    }
}