using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("EmblemBackgrounds")]
	[Serializable]
	public class EmblemBackground : IDataObject
	{
		private const String MODULE = "EmblemBackgrounds";
		public int id;
		public int order;
	}
}
