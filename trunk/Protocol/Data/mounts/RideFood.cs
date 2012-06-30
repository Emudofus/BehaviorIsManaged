using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("RideFood")]
	[Serializable]
	public class RideFood : IDataObject
	{
		public uint gid;
		public uint typeId;
		public String MODULE = "RideFood";
	}
}
