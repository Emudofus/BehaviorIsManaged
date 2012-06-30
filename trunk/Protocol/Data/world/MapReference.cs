using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("MapReferences")]
	[Serializable]
	public class MapReference : IDataObject
	{
		private const String MODULE = "MapReferences";
		public int id;
		public uint mapId;
		public int cellId;
	}
}
