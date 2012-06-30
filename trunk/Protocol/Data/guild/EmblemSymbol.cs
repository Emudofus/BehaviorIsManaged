using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("EmblemSymbols")]
	[Serializable]
	public class EmblemSymbol : IDataObject
	{
		private const String MODULE = "EmblemSymbols";
		public int id;
		public int iconId;
		public int skinId;
		public int order;
		public int categoryId;
	}
}
