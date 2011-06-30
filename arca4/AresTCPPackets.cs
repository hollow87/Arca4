using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ares.IO;

namespace arca4
{
    class AresTCPPackets
    {
        public static byte[] LoginAck(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name);
            packet.WriteString(Settings.Name);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_LOGIN_ACK);
        }

        public static byte[] TopicFirst()
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(Settings.Topic, false);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_TOPIC_FIRST);
        }

        public static byte[] Join(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(userobj.FileCount);
            packet.WriteUInt32(0);
            packet.WriteIP(userobj.ExternalIP);
            packet.WriteUInt16(userobj.Port);
            packet.WriteIP(userobj.NodeIP);
            packet.WriteUInt16(userobj.NodePort);
            packet.WriteByte(0);
            packet.WriteString(userobj.Name);
            packet.WriteIP(userobj.LocalIP);
            packet.WriteByte(userobj.CanBrowse ? (byte)1 : (byte)0);
            packet.WriteByte(userobj.Level);
            packet.WriteByte(userobj.Age);
            packet.WriteByte(userobj.Sex);
            packet.WriteByte(userobj.Country);
            packet.WriteString(userobj.Location);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_JOIN);
        }

        public static byte[] Part(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_PART);
        }

        public static byte[] UserListItem(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(userobj.FileCount);
            packet.WriteUInt32(0);
            packet.WriteIP(userobj.ExternalIP);
            packet.WriteUInt16(userobj.Port);
            packet.WriteIP(userobj.NodeIP);
            packet.WriteUInt16(userobj.NodePort);
            packet.WriteByte(0);
            packet.WriteString(userobj.Name);
            packet.WriteIP(userobj.LocalIP);
            packet.WriteByte(userobj.CanBrowse ? (byte)1 : (byte)0);
            packet.WriteByte(userobj.Level);
            packet.WriteByte(userobj.Age);
            packet.WriteByte(userobj.Sex);
            packet.WriteByte(userobj.Country);
            packet.WriteString(userobj.Location);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_CHANNEL_USER_LIST);
        }

        public static byte[] UserListBotItem()
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(0);
            packet.WriteUInt32(0);
            packet.WriteIP(IPAddress.Parse("0.0.0.0"));
            packet.WriteUInt16(0);
            packet.WriteIP(IPAddress.Parse("0.0.0.0"));
            packet.WriteUInt16(0);
            packet.WriteByte(0);
            packet.WriteString(Settings.BotName);
            packet.WriteIP(IPAddress.Parse("0.0.0.0"));
            packet.WriteByte(0);
            packet.WriteByte(3);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteString(String.Empty);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_CHANNEL_USER_LIST);
        }

        public static byte[] UserListEnd()
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteByte(0);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_CHANNEL_USER_LIST_END);
        }

        public static byte[] MyFeatures(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(Settings.Version);
            packet.WriteByte(7);
            packet.WriteByte(63);
            packet.WriteByte(Settings.Language);
            packet.WriteUInt32(userobj.Cookie);
            packet.WriteByte(1);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_MYFEATURES);
        }

    }
}
