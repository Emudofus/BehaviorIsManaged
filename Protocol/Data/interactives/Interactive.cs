

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Interactives")]
    public class Interactive : IDataObject
    {
        private const String MODULE = "Interactives";
        public int id;
        public uint nameId;
        public int actionId;
        public Boolean displayTooltip;
    }
}