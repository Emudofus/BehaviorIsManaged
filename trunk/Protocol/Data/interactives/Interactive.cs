

// Generated on 09/23/2012 21:40:20
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Interactive")]
    public class Interactive : IDataObject
    {
        private const String MODULE = "Interactives";
        public int id;
        public uint nameId;
        public int actionId;
        public Boolean displayTooltip;
    }
}