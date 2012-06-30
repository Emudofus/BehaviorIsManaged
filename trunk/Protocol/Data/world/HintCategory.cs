using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("HintCategory")]
	[Serializable]
	public class HintCategory : IDataObject
	{
		private const String MODULE = "HintCategory";
		public int id;
		public uint nameId;
	}
}
