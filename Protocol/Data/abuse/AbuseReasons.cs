

// Generated on 04/17/2013 22:30:11
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AbuseReasons")]
    public class AbuseReasons : IDataObject
    {
        public const String MODULE = "AbuseReasons";
        public uint _abuseReasonId;
        public uint _mask;
        public int _reasonTextId;
    }
}