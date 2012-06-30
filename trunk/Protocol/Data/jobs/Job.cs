using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Jobs")]
	[Serializable]
	public class Job : IDataObject
	{
		private const String MODULE = "Jobs";
		public int id;
		public uint nameId;
		public int specializationOfId;
		public int iconId;
		public List<int> toolIds;
	}
}
