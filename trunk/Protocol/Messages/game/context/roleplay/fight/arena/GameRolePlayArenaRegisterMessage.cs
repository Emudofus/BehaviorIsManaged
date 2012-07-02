// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameRolePlayArenaRegisterMessage.xml' the '27/06/2012 15:55:02'
using System;
using BiM.Core.IO;

namespace BiM.Protocol.Messages
{
	public class GameRolePlayArenaRegisterMessage : NetworkMessage
	{
		public const uint Id = 6280;
		public override uint MessageId
		{
			get
			{
				return 6280;
			}
		}
		
		public int battleMode;
		
		public GameRolePlayArenaRegisterMessage()
		{
		}
		
		public GameRolePlayArenaRegisterMessage(int battleMode)
		{
			this.battleMode = battleMode;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(battleMode);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			battleMode = reader.ReadInt();
			if ( battleMode < 0 )
			{
				throw new Exception("Forbidden value on battleMode = " + battleMode + ", it doesn't respect the following condition : battleMode < 0");
			}
		}
	}
}