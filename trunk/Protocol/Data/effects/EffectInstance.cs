using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class EffectInstance : IDataObject
	{
		public uint effectId;
		public int targetId;
		public int duration;
		public int delay;
		public int random;
		public int group;
		public int modificator;
		public Boolean trigger;
		public Boolean hidden;
		public uint zoneSize;
		public uint zoneShape;
		public uint zoneMinSize;
        public string rawZone;
	}
}
