using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("CensoredWords")]
	[Serializable]
	public class CensoredWord : IDataObject
	{
		private const String MODULE = "CensoredWords";
		public uint id;
		public uint listId;
		public String language;
		public String word;
		public Boolean deepLooking;
	}
}
