using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("AlignmentEffect")]
	[Serializable]
	public class AlignmentEffect : IDataObject
	{
		private const String MODULE = "AlignmentEffect";
		public int id;
		public uint characteristicId;
		public uint descriptionId;
	}
}
