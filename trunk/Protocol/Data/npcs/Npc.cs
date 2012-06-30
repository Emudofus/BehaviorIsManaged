using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Npcs")]
	[Serializable]
	public class Npc : IDataObject
	{
		private const String MODULE = "Npcs";
		public int id;
		public uint nameId;
		public List<List<int>> dialogMessages;
		public List<List<int>> dialogReplies;
		public List<uint> actions;
		public uint gender;
		public String look;
		public int tokenShop;
		public List<AnimFunNpcData> animFunList;
	}
}
