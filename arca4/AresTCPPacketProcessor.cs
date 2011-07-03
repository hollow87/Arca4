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

                case ProtoMessage.MSG_CHAT_CLIENT_PVT:
                    ProcessPrivateText(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_IGNORELIST:
                    ProcessIgnoreRequest(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_COMMAND:
                    ProcessCommandText(userobj, packet.ReadString());
                    break;
            }
        }

        private static void ProcessLogin(UserObject userobj, AresTCPPacketReader packet)
        {
            if (userobj.LoggedIn)
                throw new Exception();

            userobj.PopulateCredentials(packet);

            if (userobj.Name == null)
            {
                ServerEvents.OnRejected(userobj, RejectionType.NameInUse);
                userobj.Expired = true;
                return;
            }

            if (Bans.IsBanned(userobj))
            {
                ServerEvents.OnRejected(userobj, RejectionType.Banned);
                userobj.Expired = true;
                return;
            }

            if (!ServerEvents.OnJoinCheck(userobj))
            {
                ServerEvents.OnRejected(userobj, RejectionType.Script);
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
            ServerEvents.OnMOTD(userobj);
            ServerEvents.OnJoin(userobj);
        }

        private static void ProcessPublicText(UserObject userobj, AresTCPPacketReader packet)
        {
            String text = packet.ReadString();

            if (text.StartsWith("#"))
            {
                ProcessCommandText(userobj, text.Substring(1));

                if (text.Substring(1).StartsWith("login"))
                    return;
            }

            text = ServerEvents.OnTextBefore(userobj, text);

            if (!userobj.Expired)
                if (!String.IsNullOrEmpty(text))
                {
                    UserPool.BroadcastToVroom(userobj.Vroom, AresTCPPackets.Public(userobj.Name, text));
                    ServerEvents.OnTextAfter(userobj, text);
                }
        }

        private static void ProcessCommandText(UserObject userobj, String text)
        {
            CommandObject cmd = Helpers.TextToCommand(userobj, text);
            ServerEvents.OnCommand(userobj, text, cmd.target, cmd.args);
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

            if (name == Settings.BotName)
            {
                if (text.StartsWith("#") || text.StartsWith("/"))
                {
                    ProcessCommandText(userobj, text.Substring(1));

                    if (text.Substring(1).StartsWith("login"))
                        text = String.Empty;
                }

                if (!String.IsNullOrEmpty(text))
                    ServerEvents.OnBotPM(userobj, text);
            }
            else
            {
                UserObject target = UserPool.Users.Find(x => x.LoggedIn && x.Name == name);

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

        private static void ProcessIgnoreRequest(UserObject userobj, AresTCPPacketReader packet)
        {
            byte type = packet.ReadByte();
            String name = packet.ReadString();
            UserObject target = UserPool.Users.Find(x => x.LoggedIn && x.Name == name);

            if (target != null)
            {
                if (type == 0) // unignore
                {
                    if (userobj.Ignores.Contains(target.Name))
                        userobj.Ignores.Remove(target.Name);

                    userobj.SendPacket(AresTCPPackets.NoSuch(target.Name + " is unignored"));
                }
                else
                {
                    if (target.Level > 0)
                    {
                        userobj.SendPacket(AresTCPPackets.NoSuch("you cannot ignore this admin"));
                        return;
                    }

                    if (!userobj.Ignores.Contains(target.Name))
                        userobj.Ignores.Add(target.Name);

                    userobj.SendPacket(AresTCPPackets.NoSuch(target.Name + " is ignored"));
                }
            }
        }

    }
}
