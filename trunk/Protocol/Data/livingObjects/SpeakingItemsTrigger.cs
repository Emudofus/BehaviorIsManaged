

// Generated on 09/23/2012 21:40:24
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SpeakingItemsTrigger")]
    public class SpeakingItemsTrigger : IDataObject
    {
        private const String MODULE = "SpeakingItemsTriggers";
        public int triggersId;
        public List<int> textIds;
        public List<int> states;
    }
}