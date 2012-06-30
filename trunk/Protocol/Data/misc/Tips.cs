using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Tips")]
	[Serializable]
	public class Tips : IDataObject
	{
		private const String MODULE = "Tips";
		public int id;
		public uint descId;
	}
}
