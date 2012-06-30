using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("AlignmentGift")]
	[Serializable]
	public class AlignmentGift : IDataObject
	{
		private const String MODULE = "AlignmentGift";
		public int id;
		public uint nameId;
		public int effectId;
		public uint gfxId;
	}
}
