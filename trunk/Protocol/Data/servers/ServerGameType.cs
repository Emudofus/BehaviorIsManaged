using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("ServerGameTypes")]
	[Serializable]
	public class ServerGameType : IDataObject
	{
		private const String MODULE = "ServerGameTypes";
		public int id;
		public uint nameId;
	}
}
