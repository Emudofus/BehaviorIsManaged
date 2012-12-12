

// Generated on 12/11/2012 19:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ActorRestrictionsInformations
    {
        public const short Id = 204;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public bool cantBeAggressed;
        public bool cantBeChallenged;
        public bool cantTrade;
        public bool cantBeAttackedByMutant;
        public bool cantRun;
        public bool forceSlowWalk;
        public bool cantMinimize;
        public bool cantMove;
        public bool cantAggress;
        public bool cantChallenge;
        public bool cantExchange;
        public bool cantAttack;
        public bool cantChat;
        public bool cantBeMerchant;
        public bool cantUseObject;
        public bool cantUseTaxCollector;
        public bool cantUseInteractive;
        public bool cantSpeakToNPC;
        public bool cantChangeZone;
        public bool cantAttackMonster;
        public bool cantWalk8Directions;
        
        public ActorRestrictionsInformations()
        {
        }
        
        public ActorRestrictionsInformations(bool cantBeAggressed, bool cantBeChallenged, bool cantTrade, bool cantBeAttackedByMutant, bool cantRun, bool forceSlowWalk, bool cantMinimize, bool cantMove, bool cantAggress, bool cantChallenge, bool cantExchange, bool cantAttack, bool cantChat, bool cantBeMerchant, bool cantUseObject, bool cantUseTaxCollector, bool cantUseInteractive, bool cantSpeakToNPC, bool cantChangeZone, bool cantAttackMonster, bool cantWalk8Directions)
        {
            this.cantBeAggressed = cantBeAggressed;
            this.cantBeChallenged = cantBeChallenged;
            this.cantTrade = cantTrade;
            this.cantBeAttackedByMutant = cantBeAttackedByMutant;
            this.cantRun = cantRun;
            this.forceSlowWalk = forceSlowWalk;
            this.cantMinimize = cantMinimize;
            this.cantMove = cantMove;
            this.cantAggress = cantAggress;
            this.cantChallenge = cantChallenge;
            this.cantExchange = cantExchange;
            this.cantAttack = cantAttack;
            this.cantChat = cantChat;
            this.cantBeMerchant = cantBeMerchant;
            this.cantUseObject = cantUseObject;
            this.cantUseTaxCollector = cantUseTaxCollector;
            this.cantUseInteractive = cantUseInteractive;
            this.cantSpeakToNPC = cantSpeakToNPC;
            this.cantChangeZone = cantChangeZone;
            this.cantAttackMonster = cantAttackMonster;
            this.cantWalk8Directions = cantWalk8Directions;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, cantBeAggressed);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, cantBeChallenged);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, cantTrade);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, cantBeAttackedByMutant);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 4, cantRun);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 5, forceSlowWalk);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 6, cantMinimize);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 7, cantMove);
            writer.WriteByte(flag1);
            byte flag2 = 0;
            flag2 = BooleanByteWrapper.SetFlag(flag2, 0, cantAggress);
            flag2 = BooleanByteWrapper.SetFlag(flag2, 1, cantChallenge);
            flag2 = BooleanByteWrapper.SetFlag(flag2, 2, cantExchange);
            flag2 = BooleanByteWrapper.SetFlag(flag2, 3, cantAttack);
            flag2 = BooleanByteWrapper.SetFlag(flag2, 4, cantChat);
            flag2 = BooleanByteWrapper.SetFlag(flag2, 5, cantBeMerchant);
            flag2 = BooleanByteWrapper.SetFlag(flag2, 6, cantUseObject);
            flag2 = BooleanByteWrapper.SetFlag(flag2, 7, cantUseTaxCollector);
            writer.WriteByte(flag2);
            byte flag3 = 0;
            flag3 = BooleanByteWrapper.SetFlag(flag3, 0, cantUseInteractive);
            flag3 = BooleanByteWrapper.SetFlag(flag3, 1, cantSpeakToNPC);
            flag3 = BooleanByteWrapper.SetFlag(flag3, 2, cantChangeZone);
            flag3 = BooleanByteWrapper.SetFlag(flag3, 3, cantAttackMonster);
            flag3 = BooleanByteWrapper.SetFlag(flag3, 4, cantWalk8Directions);
            writer.WriteByte(flag3);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            cantBeAggressed = BooleanByteWrapper.GetFlag(flag1, 0);
            cantBeChallenged = BooleanByteWrapper.GetFlag(flag1, 1);
            cantTrade = BooleanByteWrapper.GetFlag(flag1, 2);
            cantBeAttackedByMutant = BooleanByteWrapper.GetFlag(flag1, 3);
            cantRun = BooleanByteWrapper.GetFlag(flag1, 4);
            forceSlowWalk = BooleanByteWrapper.GetFlag(flag1, 5);
            cantMinimize = BooleanByteWrapper.GetFlag(flag1, 6);
            cantMove = BooleanByteWrapper.GetFlag(flag1, 7);
            byte flag2 = reader.ReadByte();
            cantAggress = BooleanByteWrapper.GetFlag(flag2, 0);
            cantChallenge = BooleanByteWrapper.GetFlag(flag2, 1);
            cantExchange = BooleanByteWrapper.GetFlag(flag2, 2);
            cantAttack = BooleanByteWrapper.GetFlag(flag2, 3);
            cantChat = BooleanByteWrapper.GetFlag(flag2, 4);
            cantBeMerchant = BooleanByteWrapper.GetFlag(flag2, 5);
            cantUseObject = BooleanByteWrapper.GetFlag(flag2, 6);
            cantUseTaxCollector = BooleanByteWrapper.GetFlag(flag2, 7);
            byte flag3 = reader.ReadByte();
            cantUseInteractive = BooleanByteWrapper.GetFlag(flag3, 0);
            cantSpeakToNPC = BooleanByteWrapper.GetFlag(flag3, 1);
            cantChangeZone = BooleanByteWrapper.GetFlag(flag3, 2);
            cantAttackMonster = BooleanByteWrapper.GetFlag(flag3, 3);
            cantWalk8Directions = BooleanByteWrapper.GetFlag(flag3, 4);
        }
        
    }
    
}