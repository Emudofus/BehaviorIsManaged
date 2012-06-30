using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("ItemTypes")]
	[Serializable]
	public class ItemType : IDataObject
	{
		private const String MODULE = "ItemTypes";
		public int id;
		public uint nameId;
		public uint superTypeId;
		public Boolean plural;
		public uint gender;
		public String rawZone;
		public Boolean needUseConfirm;
	}
}
