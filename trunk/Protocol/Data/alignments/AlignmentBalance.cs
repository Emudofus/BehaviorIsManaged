using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("AlignmentBalance")]
	[Serializable]
	public class AlignmentBalance : IDataObject
	{
		private const String MODULE = "AlignmentBalance";
		public int id;
		public int startValue;
		public int endValue;
		public uint nameId;
		public uint descriptionId;
	}
}
