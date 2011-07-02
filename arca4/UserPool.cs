using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        private static String[] Illegal = new String[]
        {
            "￼", "", "­", "/", "\\", "www."
        };

        public static String PrepareUserName(String name, uint cookie)
        {
            String str = name;

            foreach (String i in Illegal)
                str = Regex.Replace(str, Regex.Escape(i), String.Empty, RegexOptions.IgnoreCase);

            str = str.Replace("_", " ");

            while (Encoding.UTF8.GetByteCount(str) > 20)
                str = str.Substring(0, str.Length - 1);

            bool accepted = true;

            if (Users.Find(x => x.LoggedIn && (x.Name == str || x.OrgName == str)) != null)
                accepted = false;
            else if (Encoding.UTF8.GetByteCount(str) < 2 || str == Settings.BotName)
                accepted = false;

            if (!accepted)
                str = "anon " + cookie;

            return str;
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
