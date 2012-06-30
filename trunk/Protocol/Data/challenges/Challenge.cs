using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Challenge")]
	[Serializable]
	public class Challenge : IDataObject
	{
		private const String MODULE = "Challenge";
		public int id;
		public uint nameId;
		public uint descriptionId;
	}
}
