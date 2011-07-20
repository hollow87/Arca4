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

        public static byte[] CustomFont(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name); // user's name + null
            packet.WriteByte((byte)userobj.Font.Size); // limited to between 8 to 16
            packet.WriteString(userobj.Font.Name); // null terminated
            packet.WriteByte(userobj.Font.NameColor);
            packet.WriteByte(userobj.Font.TextColor);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_CUSTOM_FONT); // id = 204
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }

        public static byte[] CustomFontDefault(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name); // user's name + null
            packet.WriteByte(10); // limited to between 8 to 16
            packet.WriteString("Verdana"); // null terminated
            packet.WriteByte(255);
            packet.WriteByte(255);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_CUSTOM_FONT); // id = 204
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }
    }
}
