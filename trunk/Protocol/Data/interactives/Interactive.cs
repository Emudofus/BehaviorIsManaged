using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Interactives")]
	[Serializable]
	public class Interactive : IDataObject
	{
		private const String MODULE = "Interactives";
		public int id;
		public uint nameId;
		public int actionId;
		public Boolean displayTooltip;
	}
}
