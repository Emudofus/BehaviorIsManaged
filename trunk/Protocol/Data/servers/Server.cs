using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Servers")]
	[Serializable]
	public class Server : IDataObject
	{
		private const String MODULE = "Servers";
		public int id;
		public uint nameId;
		public uint commentId;
		public float openingDate;
		public String language;
		public int populationId;
		public uint gameTypeId;
		public int communityId;
		public List<String> restrictedToLanguages;
	}
}
