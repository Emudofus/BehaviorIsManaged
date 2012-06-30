using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Recipes")]
	[Serializable]
	public class Recipe : IDataObject
	{
		private const String MODULE = "Recipes";
		public int resultId;
		public uint resultLevel;
		public List<int> ingredientIds;
		public List<uint> quantities;
	}
}
