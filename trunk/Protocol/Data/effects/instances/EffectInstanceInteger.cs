using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceInteger : EffectInstance, IDataObject
	{
		public int value;
	}
}
