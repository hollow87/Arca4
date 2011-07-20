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

        public static byte[] SupportsVoiceClips()
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteByte(Settings.CanVoiceChat ? (byte)1 : (byte)0);
            packet.WriteByte(0);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_VC_SUPPORTED);
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }

        public static byte[] VoiceChatUserSupport(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name);
            packet.WriteByte(userobj.CanVCPublic ? (byte)1 : (byte)0);
            packet.WriteByte(userobj.CanVCPrivate ? (byte)1 : (byte)0);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_VC_USER_SUPPORTED);
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }

        public static byte[] VoiceChatFirst(String sender, byte[] buffer)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(sender);
            packet.WriteBytes(buffer);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_VC_FIRST);
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }

        public static byte[] VoiceChatFirstTo(String sender, byte[] buffer)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(sender);
            packet.WriteBytes(buffer);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_VC_FIRST_FROM);
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }

        public static byte[] VoiceChatChunk(String sender, byte[] buffer)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(sender);
            packet.WriteBytes(buffer);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_VC_CHUNK);
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }

        public static byte[] VoiceChatChunkTo(String sender, byte[] buffer)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(sender);
            packet.WriteBytes(buffer);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_VC_CHUNK_FROM);
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }

        public static byte[] VoiceChatIgnored(String sender)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(sender);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_VC_IGNORE);
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }

        public static byte[] VoiceChatNoPrivate(String sender)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(sender);
            byte[] buf = packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_VC_NOPVT);
            packet = new AresTCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_ADVANCED_FEATURES_PROTOCOL);
        }
    }
}
