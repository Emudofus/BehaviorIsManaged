

// Generated on 10/25/2012 10:43:02
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SpellStates")]
    public class SpellState : IDataObject
    {
        private const String MODULE = "SpellStates";
        public int id;
        public uint nameId;
        public Boolean preventsSpellCast;
        public Boolean preventsFight;
        public Boolean critical;
    }
}