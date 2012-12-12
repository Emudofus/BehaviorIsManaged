

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Recipes")]
    public class Recipe : IDataObject
    {
        private const String MODULE = "Recipes";
        public int resultId;
        public uint resultLevel;
        public List<int> ingredientIds;
        public List<uint> quantities;
    }
}