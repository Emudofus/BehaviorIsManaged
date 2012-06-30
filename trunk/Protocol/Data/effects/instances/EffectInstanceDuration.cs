using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceDuration : EffectInstance, IDataObject
	{
		public uint days;
		public uint hours;
		public uint minutes;
	}
}
