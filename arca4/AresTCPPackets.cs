using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ares.IO;
using Compression;
using arca4;

namespace Ares.Protocol
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

        public static byte[] Avatar(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name);
            packet.WriteBytes(userobj.Avatar);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_AVATAR);
        }

        public static byte[] PersonalMessage(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name);
            packet.WriteString(userobj.PersonalMessage, false);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_PERSONAL_MESSAGE);
        }

        public static byte[] NoSuch()
        {
            return NoSuch(String.Empty);
        }

        public static byte[] NoSuch(String text)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();

            if (text.Length > 1024)
                text = text.Substring(0, 1024);

            packet.WriteString(text, false);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_NOSUCH);
        }

        public static byte[] OpChange(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteByte(userobj.Level > 0 ? (byte)1 : (byte)0);
            packet.WriteByte(0);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_OPCHANGE);
        }

        public static byte[] Public(String username, String text)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(username);
            packet.WriteString(text, false);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_PUBLIC);
        }

        public static byte[] Emote(String username, String text)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(username);
            packet.WriteString(text, false);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_EMOTE);
        }

        public static byte[] Private(String username, String text)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(username);
            packet.WriteString(text, false);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_PVT);
        }

        public static byte[] IsIgnoringYou(String name)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(name);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_ISIGNORINGYOU);
        }

        public static byte[] OfflineUser(String name)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(name);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_OFFLINEUSER);
        }

        public static byte[] FastPing()
        {
            return new AresTCPPacketWriter().ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_FASTPING);
        }

        public static byte[] UpdateUserStatus(UserObject userobj)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name);
            packet.WriteUInt16(userobj.FileCount);
            packet.WriteByte(userobj.CanBrowse ? (byte)1 : (byte)0);
            packet.WriteIP(userobj.NodeIP);
            packet.WriteUInt16(userobj.NodePort);
            packet.WriteIP(userobj.ExternalIP);
            packet.WriteByte(userobj.Level);
            packet.WriteByte(userobj.Age);
            packet.WriteByte(userobj.Sex);
            packet.WriteByte(userobj.Country);
            packet.WriteString(userobj.Location);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_UPDATE_USER_STATUS);
        }

        public static byte[] ClientCompressed(byte[] data)
        {
            try
            {
                AresTCPPacketWriter packet = new AresTCPPacketWriter();
                packet.WriteBytes(ZipLib.Compress(data));
                return packet.ToAresPacket(ProtoMessage.MSG_CHAT_CLIENTCOMPRESSED);
            }
            catch { }

            return new byte[] { };
        }

        public static byte[] SearchHit(ushort id, UserObject userobj, SharedItem file)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(id);
            packet.WriteByte((byte)file.Mime);
            packet.WriteUInt32(file.Size);
            packet.WriteBytes(file.Data);
            packet.WriteString(userobj.Name);
            packet.WriteIP(userobj.ExternalIP);
            packet.WriteUInt16(userobj.Port);
            packet.WriteIP(userobj.NodeIP);
            packet.WriteUInt16(userobj.NodePort);
            packet.WriteIP(userobj.LocalIP);
            packet.WriteByte(userobj.CurrentUploads);
            packet.WriteByte(userobj.MaxUploads);
            packet.WriteByte(userobj.CurrentQueued);
            packet.WriteByte(1);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_SEARCHHIT);
        }

        public static byte[] EndOfSearch(ushort id)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(id);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_ENDOFSEARCH);
        }

        public static byte[] EndOfBrowse(ushort id)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(id);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_ENDOFBROWSE);
        }

        public static byte[] BrowseError(ushort id)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(id);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_BROWSEERROR);
        }

        public static byte[] BrowseItem(ushort id, SharedItem file)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(id);
            packet.WriteByte((byte)file.Mime);
            packet.WriteUInt32(file.Size);
            packet.WriteBytes(file.Data);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_BROWSEITEM);
        }

        public static byte[] StartOfBrowse(ushort id, ushort count)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteUInt16(id);
            packet.WriteUInt16(count);
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_STARTOFBROWSE);
        }

        public static byte[] DirectChatPush(UserObject userobj, Guid guid)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();
            packet.WriteString(userobj.Name);
            packet.WriteIP(userobj.ExternalIP);
            packet.WriteUInt16(userobj.Port);
            packet.WriteIP(userobj.LocalIP);
            packet.WriteBytes(guid.ToByteArray());
            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_CLIENT_DIRCHATPUSH);
        }

        public static byte[] SuperNodes(IPEndPoint[] nodes)
        {
            AresTCPPacketWriter packet = new AresTCPPacketWriter();

            foreach (IPEndPoint ep in nodes)
            {
                packet.WriteIP(ep.Address.GetAddressBytes());
                packet.WriteUInt16((ushort)ep.Port);
            }

            return packet.ToAresPacket(ProtoMessage.MSG_CHAT_SERVER_HERE_SUPERNODES);
        }
    }
}
