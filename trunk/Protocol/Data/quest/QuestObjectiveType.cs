using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("QuestObjectiveTypes")]
	[Serializable]
	public class QuestObjectiveType : IDataObject
	{
		private const String MODULE = "QuestObjectiveTypes";
		public uint id;
		public uint nameId;
	}
}
