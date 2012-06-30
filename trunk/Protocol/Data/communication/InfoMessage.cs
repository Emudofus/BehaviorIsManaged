using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("InfoMessages")]
	[Serializable]
	public class InfoMessage : IDataObject
	{
		private const String MODULE = "InfoMessages";
		public uint typeId;
		public uint messageId;
		public uint textId;
	}
}
