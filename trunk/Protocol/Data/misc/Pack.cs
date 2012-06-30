using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Pack")]
	[Serializable]
	public class Pack : IDataObject
	{
		private const String MODULE = "Pack";
		public int id;
		public String name;
		public Boolean hasSubAreas;
	}
}
