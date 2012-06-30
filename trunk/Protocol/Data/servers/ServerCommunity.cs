using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("ServerCommunities")]
	[Serializable]
	public class ServerCommunity : IDataObject
	{
		private const String MODULE = "ServerCommunities";
		public int id;
		public uint nameId;
		public String shortId;
		public List<String> defaultCountries;
	}
}
