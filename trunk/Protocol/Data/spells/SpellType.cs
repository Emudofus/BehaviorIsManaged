using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("SpellTypes")]
	[Serializable]
	public class SpellType : IDataObject
	{
		private const String MODULE = "SpellTypes";
		public int id;
		public uint longNameId;
		public uint shortNameId;
	}
}
