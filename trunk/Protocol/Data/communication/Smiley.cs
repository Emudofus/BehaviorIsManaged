using System;using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Smileys")]
	[Serializable]
	public class Smiley : IDataObject
	{
		private const String MODULE = "Smileys";
		public uint id;
		public uint order;
		public String gfxId;
		public Boolean forPlayers;
	}
}
