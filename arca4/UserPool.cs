﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using Ares.Protocol;

namespace arca4
{
    class UserPool
    {
        public static List<UserObject> Users;

        public static void Init()
        {
            Users = new List<UserObject>();
        }

        public static void SetID(UserObject userobj)
        {
            userobj.ID = -1;
            int id = 0;

            while (true)
            {
                if (Users.Find(x => x.ID == id) != null)
                    id++;
                else
                {
                    userobj.ID = id;
                    break;
                }
            }

            if (Users.Count > 1)
                Users.Sort((x, y) => x.ID.CompareTo(y.ID));
        }

        public static void Broadcast(byte[] data)
        {
            Users.ForEach(x => { if (x.LoggedIn) x.SendPacket(data); });
        }

        public static void BroadcastToVroom(ushort vroom, byte[] data)
        {
            Users.ForEach(x => { if (x.LoggedIn && x.Vroom == vroom) x.SendPacket(data); });
        }

        public static void BroadcastToUnignoredVroom(ushort vroom, String name, byte[] data)
        {
            Users.ForEach(x =>
            {
                if (x.LoggedIn && x.Vroom == vroom)
                    if (!x.Ignores.Contains(name))
                        x.SendPacket(data);
            });
        }

        private static String[] Illegal = new String[]
        {
            "￼", "", "­", "/", "\\", "www."
        };

        public static String PrepareUserName(UserObject userobj)
        {
            String str = userobj.OrgName;

            foreach (String i in Illegal)
                str = Regex.Replace(str, Regex.Escape(i), String.Empty, RegexOptions.IgnoreCase);

            str = str.Replace("_", " ");

            while (Encoding.UTF8.GetByteCount(str) > 20)
                str = str.Substring(0, str.Length - 1);

            if (Encoding.UTF8.GetByteCount(str) < 2)
                return "anon " + userobj.Cookie;

            if (str == Settings.BotName)
                return null;

            if (Users.Find(x => x.LoggedIn && (x.Name == str || x.OrgName == str)) != null) // name in use
            {
                UserObject u = Users.Find(x => x.LoggedIn && (x.Name == str || x.OrgName == str) && (x.ExternalIP.Equals(userobj.ExternalIP) || x.Guid.Equals(userobj.Guid)));

                if (u == null)
                    return null;
                else
                {
                    u.LoggedIn = u.Vroom != 0;
                    u.Expired = true;
                    userobj.Ghost = true;
                    return str;
                }
            }

            return str;
        }

        public static bool CanChangeName(UserObject userobj, String name)
        {
            foreach (String i in Illegal)
                if (Regex.IsMatch(name, Regex.Escape(i), RegexOptions.IgnoreCase))
                    return false;

            if (Encoding.UTF8.GetByteCount(name) > 20)
                return false;

            if (Encoding.UTF8.GetByteCount(name) < 2)
                return false;

            if (name == Settings.BotName)
                return false;

            return Users.Find(x => (x.LoggedIn && x.ID != userobj.ID) && (x.Name == name || x.OrgName == name)) == null;
        }

        public static void SendFastPings(uint time)
        {
            Users.ForEach(x =>
            {
                if (x.LoggedIn)
                    if (x.FastPing && (x.LastFastPing + 5) < time)
                    {
                        x.LastFastPing = time;
                        x.SendPacket(AresTCPPackets.FastPing());
                    }
            });
        }

        public static void SendUserList(UserObject userobj)
        {
            userobj.SendPacket(AresTCPPackets.UserListBotItem());
            Users.ForEach(x => { if (x.LoggedIn && x.Vroom == userobj.Vroom) userobj.SendPacket(AresTCPPackets.UserListItem(x)); });
            userobj.SendPacket(AresTCPPackets.UserListEnd());

            Users.ForEach(x =>
            {
                if (x.LoggedIn && x.Vroom == userobj.Vroom)
                {
                    if (x.Avatar.Length > 0)
                        userobj.SendPacket(AresTCPPackets.Avatar(x));

                    if (!String.IsNullOrEmpty(x.PersonalMessage))
                        userobj.SendPacket(AresTCPPackets.PersonalMessage(x));

                    if (x.Font != null)
                        userobj.SendPacket(CustomPackets.CustomFont(x));

                    if (x.CanVCPrivate || x.CanVCPublic)
                        userobj.SendPacket(CustomPackets.VoiceChatUserSupport(x));
                }
            });
        }

        public static void SendUserFeatures(UserObject userobj)
        {
            Users.ForEach(x =>
            {
                if (x.LoggedIn && x.Vroom == userobj.Vroom && x.ID != userobj.ID)
                {
                    if (userobj.Avatar.Length > 0)
                        x.SendPacket(AresTCPPackets.Avatar(userobj));

                    if (!String.IsNullOrEmpty(userobj.PersonalMessage))
                        x.SendPacket(AresTCPPackets.PersonalMessage(userobj));

                    if (userobj.CanVCPrivate || userobj.CanVCPublic)
                        x.SendPacket(CustomPackets.VoiceChatUserSupport(userobj));
                }
            });
        }

        public static void BroadcastVoiceClip(UserObject sender, byte[] packet)
        {
            Users.ForEach(x =>
            {
                if (x.LoggedIn)
                    if (x.ID != sender.ID)
                        if (x.CanVCPublic)
                            if (!x.VCIgnores.Contains(sender.Name))
                                x.SendPacket(packet);
            });
        }

        public static void SendCustomEmotesToUser(UserObject userobj)
        {
            Users.ForEach(x =>
            {
                if (x.LoggedIn && x.Vroom == userobj.Vroom)
                    if (x.SupportsCustomEmoticons)
                        x.CustomEmoticons.ForEach(y => userobj.SendPacket(CustomPackets.CustomEmoteItem(x, y)));
            });
        }
    }
}
