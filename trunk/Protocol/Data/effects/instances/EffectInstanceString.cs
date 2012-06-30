using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceString : EffectInstance, IDataObject
	{
		public String text;
	}
}
