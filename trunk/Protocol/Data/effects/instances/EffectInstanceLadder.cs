using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceLadder : EffectInstanceCreature, IDataObject
	{
		public uint monsterCount;
	}
}
