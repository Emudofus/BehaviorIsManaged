using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class ItemCriterionOperator : IDataObject
	{
		public const String SUPERIOR = ">";
		public const String INFERIOR = "<";
		public const String EQUAL = "";
		public const String DIFFERENT = "!";
	}
}
