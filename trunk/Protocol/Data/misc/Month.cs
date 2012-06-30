using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Months")]
	[Serializable]
	public class Month : IDataObject
	{
		private const String MODULE = "Months";
		public int id;
		public uint nameId;
	}
}
