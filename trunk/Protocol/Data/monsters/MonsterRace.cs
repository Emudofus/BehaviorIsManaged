using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("MonsterRaces")]
	[Serializable]
	public class MonsterRace : IDataObject
	{
		private const String MODULE = "MonsterRaces";
		public int id;
		public int superRaceId;
		public uint nameId;
	}
}
