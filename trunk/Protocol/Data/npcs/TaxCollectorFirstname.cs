using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("TaxCollectorFirstnames")]
	[Serializable]
	public class TaxCollectorFirstname : IDataObject
	{
		private const String MODULE = "TaxCollectorFirstnames";
		public int id;
		public uint firstnameId;
	}
}
