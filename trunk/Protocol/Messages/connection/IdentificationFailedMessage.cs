// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'IdentificationFailedMessage.xml' the '27/06/2012 15:54:55'
using System;
using BiM.Core.IO;

namespace BiM.Protocol.Messages
{
	public class IdentificationFailedMessage : NetworkMessage
	{
		public const uint Id = 20;
		public override uint MessageId
		{
			get
			{
				return 20;
			}
		}
		
		public sbyte reason;
		
		public IdentificationFailedMessage()
		{
		}
		
		public IdentificationFailedMessage(sbyte reason)
		{
			this.reason = reason;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteSByte(reason);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			reason = reader.ReadSByte();
			if ( reason < 0 )
			{
				throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
			}
		}
	}
}