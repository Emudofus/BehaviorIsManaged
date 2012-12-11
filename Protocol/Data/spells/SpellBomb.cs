

// Generated on 12/11/2012 19:44:39
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SpellBombs")]
    public class SpellBomb : IDataObject
    {
        private const String MODULE = "SpellBombs";
        public int id;
        public int chainReactionSpellId;
        public int explodSpellId;
        public int wallId;
        public int instantSpellId;
        public int comboCoeff;
    }
}