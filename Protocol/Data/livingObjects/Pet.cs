

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Pets")]
    public class Pet : IDataObject
    {
        public const String MODULE = "Pets";
        public int id;
        public List<int> foodItems;
        public List<int> foodTypes;
    }
}