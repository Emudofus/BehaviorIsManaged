using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Appearances")]
	[Serializable]
	public class Appearance : IDataObject
	{
		public const String MODULE = "Appearances";
		public uint id;
		public uint type;
		public String data;
	}
}
