using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using arca4;
using Ares.IO;

namespace Ares.Protocol
{
    class CustomPackets
    {
        public static byte[] CustomData(String sender, String ident, byte[] data)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(ident);
            packet.WriteString(sender);
            packet.WriteBytes(data);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_CUSTOM_DATA);
        }
    }
}
