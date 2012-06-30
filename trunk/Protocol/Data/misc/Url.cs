using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Url")]
	[Serializable]
	public class Url : IDataObject
	{
		private const String MODULE = "Url";
		public int id;
		public int browserId;
		public String url;
		public String param;
		public String method;
	}
}
