using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("IncarnationLevels")]
	[Serializable]
	public class IncarnationLevel : IDataObject
	{
		private const String MODULE = "IncarnationLevels";
		public int id;
		public int incarnationId;
		public int level;
		public uint requiredXp;
	}
}
