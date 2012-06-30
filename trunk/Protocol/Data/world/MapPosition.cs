using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("MapPositions")]
	[Serializable]
	public class MapPosition : IDataObject
	{
		private const String MODULE = "MapPositions";
		public int id;
		public int posX;
		public int posY;
		public Boolean outdoor;
		public int capabilities;
		public int nameId;
		public List<AmbientSound> sounds;
		public int subAreaId;
		public int worldMap;
		public Boolean hasPriorityOnWorldmap;
	}
}
