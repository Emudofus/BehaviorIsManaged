

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightOptionToggleMessage : NetworkMessage
    {
        public const uint Id = 707;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte option;
        
        public GameFightOptionToggleMessage()
        {
        }
        
        public GameFightOptionToggleMessage(sbyte option)
        {
            this.option = option;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(option);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            option = reader.ReadSByte();
            if (option < 0)
                throw new Exception("Forbidden value on option = " + option + ", it doesn't respect the following condition : option < 0");
        }
        
    }
    
}