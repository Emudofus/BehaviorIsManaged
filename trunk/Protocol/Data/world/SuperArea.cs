using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("SuperAreas")]
	[Serializable]
	public class SuperArea : IDataObject
	{
		private const String MODULE = "SuperAreas";
		public int id;
		public uint nameId;
		public uint worldmapId;
	}
}
