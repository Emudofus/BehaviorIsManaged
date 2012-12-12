

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Pets")]
    public class Pet : IDataObject
    {
        private const String MODULE = "Pets";
        public int id;
        public List<int> foodItems;
        public List<int> foodTypes;
    }
}