using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("ChatChannels")]
	[Serializable]
	public class ChatChannel : IDataObject
	{
		private const String MODULE = "ChatChannels";
		public uint id;
		public uint nameId;
		public uint descriptionId;
		public String shortcut;
		public String shortcutKey;
		public Boolean isPrivate;
		public Boolean allowObjects;
	}
}
