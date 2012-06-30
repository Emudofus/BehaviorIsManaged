using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("EmblemSymbolCategories")]
	[Serializable]
	public class EmblemSymbolCategory : IDataObject
	{
		private const String MODULE = "EmblemSymbolCategories";
		public int id;
		public uint nameId;
	}
}
