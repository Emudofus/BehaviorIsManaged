#region License GNU GPL
// NetworkMessage.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using BiM.Core.IO;
using BiM.Core.Messages;
using NLog;

namespace BiM.Core.Network
{
    [Serializable]
    public abstract class NetworkMessage : Message
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;

        public ListenerEntry Destinations
        {
            get;
            set;
        }

        public ListenerEntry From
        {
            get;
            set;
        }

        public abstract uint MessageId { get; }

        public override void BlockProgression()
        {
            Canceled = true;
            Destinations = ListenerEntry.Local;
        }

        public void BlockNetworkSend()
        {
            Destinations = ListenerEntry.Local;

            logger.Debug("Block message {0}", this);
        }

        public void Unpack(IDataReader reader)
        {
            Deserialize(reader);
        }

        public void Pack(IDataWriter writer)
        {
            Serialize(writer);
            WritePacket(writer);
        }

        public abstract void Serialize(IDataWriter writer);
        public abstract void Deserialize(IDataReader reader);

        private void WritePacket(IDataWriter writer)
        {
            byte[] packet = writer.Data;

            writer.Clear();

            byte typeLen = ComputeTypeLen(packet.Length);
            var header = (short) SubComputeStaticHeader(MessageId, typeLen);
            writer.WriteShort(header);

            switch (typeLen)
            {
                case 0:
                    break;
                case 1:
                    writer.WriteByte((byte) packet.Length);
                    break;
                case 2:
                    writer.WriteShort((short) packet.Length);
                    break;
                case 3:
                    writer.WriteByte((byte) (packet.Length >> 16 & 255));
                    writer.WriteShort((short) (packet.Length & 65535));
                    break;
                default:

                    throw new Exception("Packet's length can't be encoded on 4 or more bytes");
            }
            writer.WriteBytes(packet);
        }


        private static byte ComputeTypeLen(int param1)
        {
            if (param1 > 65535)
                return 3;

            if (param1 > 255)
                return 2;

            if (param1 > 0)
                return 1;

            return 0;
        }

        private static uint SubComputeStaticHeader(uint id, byte typeLen)
        {
            return id << BIT_RIGHT_SHIFT_LEN_PACKET_ID | typeLen;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}