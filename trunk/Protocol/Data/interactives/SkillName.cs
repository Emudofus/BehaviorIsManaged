using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("SkillNames")]
	[Serializable]
	public class SkillName : IDataObject
	{
		private const String MODULE = "SkillNames";
		public int id;
		public uint nameId;
	}
}
