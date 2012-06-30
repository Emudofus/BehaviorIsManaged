using System;
using System.Collections.Generic;
using BiM.Protocol.Data;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("SubAreas")]
	[Serializable]
	public class SubArea : IDataObject
	{
		private const String MODULE = "SubAreas";
		public int id;
		public uint nameId;
		public int areaId;
		public List<AmbientSound> ambientSounds;
		public List<uint> mapIds;
		public Rectangle bounds;
		public List<int> shape;
		public List<uint> customWorldMap;
		public int packId;
	}
}
