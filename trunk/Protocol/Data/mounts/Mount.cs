using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Mounts")]
	[Serializable]
	public class Mount : IDataObject
	{
		public uint id;
		public uint nameId;
		public String look;
		private String MODULE = "Mounts";
	}
}
