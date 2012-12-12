

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AbuseReasons")]
    public class AbuseReasons : IDataObject
    {
        private const String MODULE = "AbuseReasons";
        public uint _abuseReasonId;
        public uint _mask;
        public int _reasonTextId;
    }
}