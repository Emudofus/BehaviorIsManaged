using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("SpellStates")]
	[Serializable]
	public class SpellState : IDataObject
	{
		private const String MODULE = "SpellStates";
		public int id;
		public uint nameId;
		public Boolean preventsSpellCast;
		public Boolean preventsFight;
		public Boolean critical;
	}
}
