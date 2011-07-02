using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;

namespace arca4
{
    class AresTCPPacketProcessor
    {
        public static void Evaluate(UserObject userobj, ProtoMessage msg, AresTCPPacketReader packet, uint time)
        {
            if (!userobj.LoggedIn)
                if (msg > ProtoMessage.MSG_CHAT_CLIENT_LOGIN)
                    throw new Exception();

            switch (msg)
            {
                case ProtoMessage.MSG_CHAT_CLIENT_LOGIN:
                    ProcessLogin(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_UPDATE_STATUS:
                    userobj.Pinged(time);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_AVATAR:
                    if (ServerEvents.OnAvatarReceived(userobj))
                        if (!userobj.Expired)
                            userobj.Avatar = packet.ReadBytes();
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_PERSONAL_MESSAGE:
                    if (ServerEvents.OnPersonalMessageReceived(userobj))
                        if (!userobj.Expired)
                            userobj.PersonalMessage = packet.ReadString();
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_PUBLIC:
                    ProcessPublicText(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_EMOTE:
                    ProcessEmoteText(userobj, packet);
                    break;
            }
        }

        private static void ProcessLogin(UserObject userobj, AresTCPPacketReader packet)
        {
            if (userobj.LoggedIn)
                throw new Exception();

            userobj.PopulateCredentials(packet);

            if (!ServerEvents.OnJoinCheck(userobj))
            {
                ServerEvents.OnRejected(userobj);
                userobj.Expired = true;
                return;
            }

            UserPool.BroadcastToVroom(userobj.Vroom, AresTCPPackets.Join(userobj));
            userobj.LoggedIn = true;
            userobj.SendPacket(AresTCPPackets.LoginAck(userobj));
            userobj.SendPacket(AresTCPPackets.MyFeatures(userobj));
            userobj.SendPacket(AresTCPPackets.TopicFirst());
            UserPool.SendUserList(userobj);
            userobj.SendPacket(AresTCPPackets.OpChange(userobj));
            ServerEvents.OnJoin(userobj);
        }

        private static void ProcessPublicText(UserObject userobj, AresTCPPacketReader packet)
        {
            String text = packet.ReadString();
            text = ServerEvents.OnTextBefore(userobj, text);

            if (!userobj.Expired)
                if (!String.IsNullOrEmpty(text))
                {
                    UserPool.BroadcastToVroom(userobj.Vroom, AresTCPPackets.Public(userobj.Name, text));
                    ServerEvents.OnTextAfter(userobj, text);
                }
        }

        private static void ProcessEmoteText(UserObject userobj, AresTCPPacketReader packet)
        {
            String text = packet.ReadString();
            text = ServerEvents.OnEmoteBefore(userobj, text);

            if (!userobj.Expired)
                if (!String.IsNullOrEmpty(text))
                {
                    UserPool.BroadcastToVroom(userobj.Vroom, AresTCPPackets.Emote(userobj.Name, text));
                    ServerEvents.OnEmoteAfter(userobj, text);
                }
        }

        private static void ProcessPrivateText(UserObject userobj, AresTCPPacketReader packet)
        {
            String name = packet.ReadString();
            String text = packet.ReadString();
            UserObject target = UserPool.Users.Find(x => x.Name == name);

            if (target == null)
                userobj.SendPacket(AresTCPPackets.OfflineUser(name));
            else if (target.Ignores.Contains(userobj.Name))
                userobj.SendPacket(AresTCPPackets.IsIgnoringYou(name));
            else
            {
                text = ServerEvents.OnPM(userobj, target, text);

                if (!userobj.Expired)
                    if (!String.IsNullOrEmpty(text))
                        target.SendPacket(AresTCPPackets.Private(userobj.Name, text));
            }
        }

    }
}
