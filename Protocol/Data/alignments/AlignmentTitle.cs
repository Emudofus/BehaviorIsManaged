

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentTitles")]
    public class AlignmentTitle : IDataObject
    {
        private const String MODULE = "AlignmentTitles";
        public int sideId;
        public List<int> namesId;
        public List<int> shortsId;
    }
}