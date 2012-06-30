using System;
using System.Collections.Generic;
using BiM.Protocol.Data;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("QuestObjectives")]
	[Serializable]
	public class QuestObjective : IDataObject
	{
		private const String MODULE = "QuestObjectives";
		public uint id;
		public uint stepId;
		public uint typeId;
		public int dialogId;
		public List<uint> parameters;
		public Point coords;
		public QuestObjectiveType type;
	}
}
