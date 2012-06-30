using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("SoundBones")]
	[Serializable]
	public class SoundBones : IDataObject
	{
		public uint id;
		public List<String> keys;
		public List<List<SoundAnimation>> values;
		public String MODULE = "SoundBones";
	}
}
