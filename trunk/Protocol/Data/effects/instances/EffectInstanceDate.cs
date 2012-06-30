using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstanceDate : EffectInstance, IDataObject
	{
		public uint year;
		public uint month;
		public uint day;
		public uint hour;
		public uint minute;
	}
}
