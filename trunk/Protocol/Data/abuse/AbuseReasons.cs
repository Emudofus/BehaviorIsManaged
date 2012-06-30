using System;
using BiM.Protocol.Data;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("AbuseReasons")]
	[Serializable]
    public class AbuseReasons : IDataObject
	{
		private const String MODULE = "AbuseReasons";
		public uint _abuseReasonId;
		public uint _mask;
		public int _reasonTextId;
	}
}
