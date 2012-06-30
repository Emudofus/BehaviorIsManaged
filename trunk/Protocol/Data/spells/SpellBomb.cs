using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("SpellBombs")]
	[Serializable]
	public class SpellBomb : IDataObject
	{
		private const String MODULE = "SpellBombs";
		public int id;
		public int chainReactionSpellId;
		public int explodSpellId;
		public int wallId;
		public int instantSpellId;
		public int comboCoeff;
	}
}
