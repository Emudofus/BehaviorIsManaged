using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("RankNames")]
	[Serializable]
	public class RankName : IDataObject
	{
		private const String MODULE = "RankNames";
		public int id;
		public uint nameId;
		public int order;
	}
}
