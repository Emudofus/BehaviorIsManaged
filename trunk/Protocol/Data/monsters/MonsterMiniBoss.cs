using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("MonsterMiniBoss")]
	[Serializable]
	public class MonsterMiniBoss : IDataObject
	{
		private const String MODULE = "MonsterMiniBoss";
		public int id;
		public int monsterReplacingId;
	}
}
