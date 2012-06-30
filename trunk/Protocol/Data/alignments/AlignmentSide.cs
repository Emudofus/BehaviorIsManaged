using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("AlignmentSides")]
	[Serializable]
	public class AlignmentSide : IDataObject
	{
		private const String MODULE = "AlignmentSides";
		public int id;
		public uint nameId;
		public Boolean canConquest;
	}
}
