using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

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

        public static void BroadcastToVroom(ushort vroom, byte[] data)
        {
            Users.FindAll(x => x.LoggedIn && x.Vroom == vroom).ForEach(x => x.SendPacket(data));
        }

        public static void BroadcastToUnignoredVroom(ushort vroom, String name, byte[] data)
        {
            Users.FindAll(x => x.LoggedIn && x.Vroom == vroom).ForEach(x =>
            {
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
            Users.FindAll(x => x.LoggedIn && x.FastPing && (x.LastFastPing + 5) < time).ForEach(x =>
            {
                x.LastFastPing = time;
                x.SendPacket(AresTCPPackets.FastPing());
            });
        }

        public static void SendUserList(UserObject userobj)
        {
            userobj.SendPacket(AresTCPPackets.UserListBotItem());
            Users.FindAll(x => x.LoggedIn && x.Vroom == userobj.Vroom).ForEach(x => userobj.SendPacket(AresTCPPackets.UserListItem(x)));
            userobj.SendPacket(AresTCPPackets.UserListEnd());

            Users.FindAll(x => x.LoggedIn && x.Vroom == userobj.Vroom).ForEach(x =>
            {
                if (x.Avatar.Length > 0)
                    userobj.SendPacket(AresTCPPackets.Avatar(x));

                if (!String.IsNullOrEmpty(x.PersonalMessage))
                    userobj.SendPacket(AresTCPPackets.PersonalMessage(x));
            });
        }
    }
}
