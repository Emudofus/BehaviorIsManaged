using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("ServerPopulations")]
	[Serializable]
	public class ServerPopulation : IDataObject
	{
		private const String MODULE = "ServerPopulations";
		public int id;
		public uint nameId;
		public int weight;
	}
}
