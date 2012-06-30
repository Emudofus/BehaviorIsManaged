using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("CensoredContents")]
	[Serializable]
	public class CensoredContent : IDataObject
	{
		public const String MODULE = "CensoredContents";
		public int type;
		public int oldValue;
		public int newValue;
		public String lang;
	}
}
