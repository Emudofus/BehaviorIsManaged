using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("AlignmentOrder")]
	[Serializable]
	public class AlignmentOrder : IDataObject
	{
		private const String MODULE = "AlignmentOrder";
		public int id;
		public uint nameId;
		public uint sideId;
	}
}
