using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("TypeActions")]
	[Serializable]
	public class TypeAction : IDataObject
	{
		public const String MODULE = "TypeActions";
		public int id;
		public String elementName;
		public int elementId;
	}
}
