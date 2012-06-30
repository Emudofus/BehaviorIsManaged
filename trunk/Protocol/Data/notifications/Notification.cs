using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Notifications")]
	[Serializable]
	public class Notification : IDataObject
	{
		private const String MODULE = "Notifications";
		public int id;
		public uint titleId;
		public uint messageId;
		public int iconId;
		public int typeId;
		public String trigger;
	}
}
