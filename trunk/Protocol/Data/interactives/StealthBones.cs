using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("StealthBones")]
	[Serializable]
	public class StealthBones : IDataObject
	{
		private const String MODULE = "StealthBones";
		public uint id;
	}
}
