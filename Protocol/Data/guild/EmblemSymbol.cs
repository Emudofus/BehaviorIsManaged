

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("EmblemSymbols")]
    public class EmblemSymbol : IDataObject
    {
        private const String MODULE = "EmblemSymbols";
        public int id;
        public int iconId;
        public int skinId;
        public int order;
        public int categoryId;
        public Boolean colorizable;
    }
}