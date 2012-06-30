using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("OptionalFeatures")]
	[Serializable]
	public class OptionalFeature : IDataObject
	{
		public const String MODULE = "OptionalFeatures";
		public int id;
		public String keyword;
	}
}
