using System;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[Serializable]
	public class AnimFunMonsterData : AnimFunData, IDataObject
    {
        
	}

    public class AnimFunData : IDataObject
    {
        public string animName;
        public int animWeight;
    }
}
