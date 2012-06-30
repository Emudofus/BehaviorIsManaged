using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("MountBehaviors")]
	[Serializable]
	public class MountBehavior : IDataObject
	{
		public const String MODULE = "MountBehaviors";
		public uint id;
		public uint nameId;
		public uint descriptionId;
	}
}
