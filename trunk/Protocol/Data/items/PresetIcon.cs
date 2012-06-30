using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("PresetIcons")]
	[Serializable]
	public class PresetIcon : IDataObject
	{
		private const String MODULE = "PresetIcons";
		public int id;
		public int order;
	}
}
