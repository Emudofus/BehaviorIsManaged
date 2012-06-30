using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("QuestCategory")]
	[Serializable]
	public class QuestCategory : IDataObject
	{
		private const String MODULE = "QuestCategory";
		public uint id;
		public uint nameId;
		public uint order;
		public List<uint> questIds;
	}
}
