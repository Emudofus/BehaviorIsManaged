using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceMount : EffectInstance, IDataObject
	{
		public float date;
		public uint modelId;
		public uint mountId;
	}
}
