using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Pets")]
	[Serializable]
	public class Pet : IDataObject
	{
		private const String MODULE = "Pets";
		public int id;
		public List<int> foodItems;
		public List<int> foodTypes;
	}
}
