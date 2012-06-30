using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("TaxCollectorNames")]
	[Serializable]
	public class TaxCollectorName : IDataObject
	{
		private const String MODULE = "TaxCollectorNames";
		public int id;
		public uint nameId;
	}
}
