using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceCreature : EffectInstance, IDataObject
	{
		public uint monsterFamilyId;
	}
}
