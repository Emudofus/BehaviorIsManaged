using System;
using BiM.Protocol.Data;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class Weapon : Item, IDataObject
	{
		public int apCost;
		public int minRange;
		public int range;
		public Boolean castInLine;
		public Boolean castInDiagonal;
		public Boolean castTestLos;
		public int criticalHitProbability;
		public int criticalHitBonus;
		public int criticalFailureProbability;
	}
}
