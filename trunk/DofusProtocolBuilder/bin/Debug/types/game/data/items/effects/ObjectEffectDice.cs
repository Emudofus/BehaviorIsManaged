

// Generated on 10/25/2012 10:42:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ObjectEffectDice : ObjectEffect
    {
        public const short Id = 73;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short diceNum;
        public short diceSide;
        public short diceConst;
        
        public ObjectEffectDice()
        {
        }
        
        public ObjectEffectDice(short actionId, short diceNum, short diceSide, short diceConst)
         : base(actionId)
        {
            this.diceNum = diceNum;
            this.diceSide = diceSide;
            this.diceConst = diceConst;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(diceNum);
            writer.WriteShort(diceSide);
            writer.WriteShort(diceConst);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            diceNum = reader.ReadShort();
            if (diceNum < 0)
                throw new Exception("Forbidden value on diceNum = " + diceNum + ", it doesn't respect the following condition : diceNum < 0");
            diceSide = reader.ReadShort();
            if (diceSide < 0)
                throw new Exception("Forbidden value on diceSide = " + diceSide + ", it doesn't respect the following condition : diceSide < 0");
            diceConst = reader.ReadShort();
            if (diceConst < 0)
                throw new Exception("Forbidden value on diceConst = " + diceConst + ", it doesn't respect the following condition : diceConst < 0");
        }
        
    }
    
}