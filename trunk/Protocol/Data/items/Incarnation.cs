using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Incarnation")]
	[Serializable]
	public class Incarnation : IDataObject
	{
		private const String MODULE = "Incarnation";
		public uint id;
		public String lookMale;
		public String lookFemale;
	}
}
