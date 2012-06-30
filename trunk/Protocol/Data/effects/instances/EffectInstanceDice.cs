using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceDice : EffectInstanceInteger, IDataObject
	{
		public uint diceNum;
		public uint diceSide;
	}
}
