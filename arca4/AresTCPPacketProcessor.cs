using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
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
                case ProtoMessage.MSG_CHAT_CLIENT_RELOGIN:
                    userobj.FastPing = true;
                    goto case ProtoMessage.MSG_CHAT_CLIENT_LOGIN;

                case ProtoMessage.MSG_CHAT_CLIENT_LOGIN:
                    ProcessLogin(userobj, packet, time);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_FASTPING:
                    userobj.FastPing = true;
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

                case ProtoMessage.MSG_CHAT_CLIENT_AUTHLOGIN:
                    ProcessCommandText(userobj, "login " + packet.ReadString());
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_ADDSHARE:
                    ProcessAddSharePacket(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_REMSHARE:
                    ProcessRemoveSharePacket(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_SEARCH:
                    ProcessSearchPacket(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_BROWSE:
                    ProcessBrowsePacket(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_AUTHREGISTER:
                    ProcessCommandText(userobj, "register " + packet.ReadString());
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_DIRCHATPUSH:
                    ProcessDirectChatPacket(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_DUMMY:
                    ProcessDummyPacket(userobj);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_SEND_SUPERNODES:
                    ProcessSuperNodesPacket(userobj);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_AUTOLOGIN:
                    UserAccounts.Login(userobj, packet.ReadBytes());
                    break;
            }
        }

        private static String[] built_in_commands = { "login", "register", "unregister" };

        private static void ProcessLogin(UserObject userobj, AresTCPPacketReader packet, uint time)
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

            if (UserPool.Users.FindAll(x => x.ExternalIP.Equals(userobj.ExternalIP)).Count > 3)
            {
                ServerEvents.OnRejected(userobj, RejectionType.ClientExcess);
                userobj.Expired = true;
                userobj.LoggedIn = userobj.Ghost;
                return;
            }

            if (UserRecordManager.IsJoinFlooding(userobj, time))
            {
                ServerEvents.OnRejected(userobj, RejectionType.JoinFlood);
                userobj.Expired = true;
                userobj.LoggedIn = userobj.Ghost;
                return;
            }

            UserRecordManager.AddItem(userobj);

            if (Bans.IsBanned(userobj))
            {
                ServerEvents.OnRejected(userobj, RejectionType.Banned);
                userobj.Expired = true;
                userobj.LoggedIn = userobj.Ghost;
                return;
            }

            if (!ServerEvents.OnJoinCheck(userobj))
            {
                ServerEvents.OnRejected(userobj, RejectionType.Script);
                userobj.Expired = true;
                userobj.LoggedIn = userobj.Ghost;
                return;
            }

            if (!userobj.Ghost)
                UserPool.BroadcastToVroom(userobj.Vroom, AresTCPPackets.Join(userobj));

            userobj.LoggedIn = true;
            userobj.SendPacket(AresTCPPackets.LoginAck(userobj));
            userobj.SendPacket(AresTCPPackets.MyFeatures(userobj));
            userobj.SendPacket(AresTCPPackets.TopicFirst());
            UserPool.SendUserList(userobj);
            userobj.SendPacket(AresTCPPackets.OpChange(userobj));

            if (!userobj.FastPing)
                ServerEvents.OnMOTD(userobj);

            ServerEvents.OnJoin(userobj);
        }

        private static void ProcessPublicText(UserObject userobj, AresTCPPacketReader packet)
        {
            String text = packet.ReadString();

            if (text.StartsWith("#"))
            {
                ProcessCommandText(userobj, text.Substring(1));

                foreach (String str in built_in_commands)
                    if (text.Substring(1).StartsWith(str))
                        return;
            }

            text = ServerEvents.OnTextBefore(userobj, text);

            if (!userobj.Expired)
                if (!String.IsNullOrEmpty(text))
                {
                    UserPool.BroadcastToUnignoredVroom(userobj.Vroom, userobj.Name, AresTCPPackets.Public(userobj.Name, text));
                    ServerEvents.OnTextAfter(userobj, text);
                }
        }

        private static void ProcessCommandText(UserObject userobj, String text)
        {
            if (text == "help")
                ServerEvents.OnHelp(userobj);
            else if (text.StartsWith("register "))
                UserAccounts.Register(userobj, text.Substring(9));
            else if (text.StartsWith("unregister "))
                UserAccounts.Unregister(userobj, text.Substring(11));
            else if (text.StartsWith("login "))
                UserAccounts.Login(userobj, text.Substring(6));
            else
            {
                CommandObject cmd = Helpers.TextToCommand(userobj, text);
                ServerEvents.OnCommand(userobj, text, cmd.target, cmd.args);
            }
        }

        private static void ProcessEmoteText(UserObject userobj, AresTCPPacketReader packet)
        {
            String text = packet.ReadString();
            text = ServerEvents.OnEmoteBefore(userobj, text);

            if (!userobj.Expired)
                if (!String.IsNullOrEmpty(text))
                {
                    UserPool.BroadcastToUnignoredVroom(userobj.Vroom, userobj.Name, AresTCPPackets.Emote(userobj.Name, text));
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

                    foreach (String str in built_in_commands)
                        if (text.Substring(1).StartsWith(str))
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

        private static void ProcessAddSharePacket(UserObject userobj, AresTCPPacketReader packet)
        {
            if (userobj.Files.Count > 12000)
            {
                userobj.Expired = true;
                return;
            }

            SharedItem item = new SharedItem(packet);
            userobj.Files.Add(item);

            if (item.CanScript)
                ServerEvents.OnFileReceived(userobj, item.FileName, item.Title);
        }

        private static void ProcessRemoveSharePacket(UserObject userobj, AresTCPPacketReader packet)
        {
            uint size = packet.ReadUInt32();
            userobj.Files.RemoveAll(s => s.Size == size);
        }

        private static void ProcessSearchPacket(UserObject userobj, AresTCPPacketReader packet)
        {
            SearchRequest request = new SearchRequest(packet);
            List<byte[]> packets = new List<byte[]>();

            UserPool.Users.FindAll(x => x.LoggedIn && x.CanBrowse).ForEach(x => x.Files.FindAll(y => y.SearchWords.Contains(request.SearchWord)
                && request.AllTypes ? true : y.Mime == request.Mime).ForEach(y => packets.Add(AresTCPPackets.SearchHit(request.ID, x, y))));

            List<byte> p = new List<byte>();

            packets.ForEach(x =>
            {
                p.AddRange(x);

                if (p.Count > 800)
                {
                    userobj.SendPacket(AresTCPPackets.ClientCompressed(p.ToArray()));
                    p.Clear();
                }
            });

            if (p.Count > 0)
                userobj.SendPacket(AresTCPPackets.ClientCompressed(p.ToArray()));

            userobj.SendPacket(AresTCPPackets.EndOfSearch(request.ID));
        }

        private static void ProcessBrowsePacket(UserObject userobj, AresTCPPacketReader packet)
        {
            BrowseRequest request = new BrowseRequest(packet);

            if (request.Target != null)
            {
                List<byte[]> packets = new List<byte[]>();

                request.Target.Files.FindAll(x => request.AllTypes ? true : x.Mime == request.Mime).ForEach(x =>
                    packets.Add(AresTCPPackets.BrowseItem(request.ID, x)));

                userobj.SendPacket(AresTCPPackets.StartOfBrowse(request.ID, (ushort)packets.Count));
                
                List<byte> p = new List<byte>();

                packets.ForEach(x =>
                {
                    p.AddRange(x);

                    if (p.Count > 800)
                    {
                        userobj.SendPacket(AresTCPPackets.ClientCompressed(p.ToArray()));
                        p.Clear();
                    }
                });
                
                if (p.Count > 0)
                    userobj.SendPacket(AresTCPPackets.ClientCompressed(p.ToArray()));

                userobj.SendPacket(AresTCPPackets.EndOfBrowse(request.ID));
            }
            else userobj.SendPacket(AresTCPPackets.BrowseError(request.ID));
        }

        private static void ProcessDirectChatPacket(UserObject userobj, AresTCPPacketReader packet)
        {
            String name = packet.ReadString();
            Guid guid = packet.ReadGuid();
            UserObject target = UserPool.Users.Find(x => x.Name == name);

            if (target == null)
                userobj.SendPacket(new byte[] { 1, 0, (byte)ProtoMessage.MSG_CHAT_CLIENT_DIRCHATPUSH, 1 });
            else if (target.Ignores.Contains(userobj.Name))
                userobj.SendPacket(new byte[] { 1, 0, (byte)ProtoMessage.MSG_CHAT_CLIENT_DIRCHATPUSH, 2 });
            else
            {
                userobj.SendPacket(new byte[] { 1, 0, (byte)ProtoMessage.MSG_CHAT_CLIENT_DIRCHATPUSH, 0 });
                target.SendPacket(AresTCPPackets.DirectChatPush(userobj, guid));
            }
        }

        private static void ProcessDummyPacket(UserObject userobj)
        {
            Bans.AddBan(userobj);
            userobj.Expired = true;
        }

        private static void ProcessSuperNodesPacket(UserObject userobj)
        {
            List<IPEndPoint> endpoints = new List<IPEndPoint>();
            
            UserPool.Users.FindAll(x => x.LoggedIn).ForEach(x =>
            {
                if (!x.NodeIP.Equals(IPAddress.Parse("0.0.0.0")))
                    if (x.NodePort > 0)
                        endpoints.Add(new IPEndPoint(x.NodeIP, x.NodePort));
            });

            userobj.SendPacket(AresTCPPackets.SuperNodes(endpoints.ToArray()));
        }
    }
}
