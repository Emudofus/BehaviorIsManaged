using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("SpeakingItemsText")]
	[Serializable]
	public class SpeakingItemText : IDataObject
	{
		private const String MODULE = "SpeakingItemsText";
		public int textId;
		public float textProba;
		public uint textStringId;
		public int textLevel;
		public int textSound;
		public String textRestriction;
	}
}
