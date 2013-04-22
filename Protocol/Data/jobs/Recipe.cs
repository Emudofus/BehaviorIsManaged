

// Generated on 04/17/2013 22:30:14
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Recipes")]
    public class Recipe : IDataObject
    {
        public const String MODULE = "Recipes";
        public int resultId;
        public uint resultLevel;
        public List<int> ingredientIds;
        public List<uint> quantities;
    }
}