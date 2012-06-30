using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Titles")]
	[Serializable]
	public class Title : IDataObject
	{
		private const String MODULE = "Titles";
		public int id;
		public uint nameId;
		public String color;
	}
}
