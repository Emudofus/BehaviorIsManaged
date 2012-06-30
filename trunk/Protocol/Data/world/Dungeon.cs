using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Dungeons")]
	[Serializable]
	public class Dungeon : IDataObject
	{
		private const String MODULE = "Dungeons";
		public int id;
		public uint nameId;
		public int optimalPlayerLevel;
	}
}
