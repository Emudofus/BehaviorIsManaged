using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("NpcMessages")]
	[Serializable]
	public class NpcMessage : IDataObject
	{
		private const String MODULE = "NpcMessages";
		public int id;
		public uint messageId;
		public String messageParams;
	}
}
