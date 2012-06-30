using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("AlignmentRank")]
	[Serializable]
	public class AlignmentRank : IDataObject
	{
		private const String MODULE = "AlignmentRank";
		public int id;
		public uint orderId;
		public uint nameId;
		public uint descriptionId;
		public int minimumAlignment;
		public int objectsStolen;
		public List<int> gifts;
	}
}
