using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceMinMax : EffectInstance, IDataObject
	{
		public uint min;
		public uint max;
	}
}
