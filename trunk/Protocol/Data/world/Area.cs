using System;
using BiM.Protocol.Data;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Areas")]
	[Serializable]
	public class Area : IDataObject
	{
		private const String MODULE = "Areas";
		public int id;
		public uint nameId;
		public int superAreaId;
		public Boolean containHouses;
		public Boolean containPaddocks;
		public Rectangle bounds;
	}
}
