using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Documents")]
	[Serializable]
	public class Document : IDataObject
	{
		private const String MODULE = "Documents";
		public int id;
		public uint typeId;
		public uint titleId;
		public uint authorId;
		public uint subTitleId;
		public uint contentId;
		public String contentCSS;
	}
}
