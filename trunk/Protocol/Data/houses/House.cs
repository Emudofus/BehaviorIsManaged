using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Houses")]
	[Serializable]
	public class House : IDataObject
	{
		private const String MODULE = "Houses";
		public int typeId;
		public uint defaultPrice;
		public int nameId;
		public int descriptionId;
		public int gfxId;
	}
}
