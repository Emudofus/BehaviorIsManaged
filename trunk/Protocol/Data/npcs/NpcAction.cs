using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("NpcActions")]
	[Serializable]
	public class NpcAction : IDataObject
	{
		private const String MODULE = "NpcActions";
		public int id;
		public uint nameId;
	}
}
