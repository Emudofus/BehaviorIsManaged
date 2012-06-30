using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("MonsterSuperRaces")]
	[Serializable]
	public class MonsterSuperRace : IDataObject
	{
		private const String MODULE = "MonsterSuperRaces";
		public int id;
		public uint nameId;
	}
}
