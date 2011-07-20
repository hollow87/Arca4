using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using Ares.Protocol;

namespace arca4
{
    class Bans
    {
        private static List<UserRecordItem> Items = new List<UserRecordItem>();

        public static bool IsBanned(UserObject userobj)
        {
            return Items.Find(x => x.ExternalIP.Equals(userobj.ExternalIP) || x.Guid.Equals(userobj.Guid)) != null;
        }

        public static void AddBan(UserObject userobj)
        {
            Items.Add(new UserRecordItem(userobj, Helpers.UnixTime));
        }

        public static String RemoveBan(String name)
        {
            int i = Items.FindIndex(x => x.Name.StartsWith(name));

            if (i > -1)
            {
                String result = Items[i].Name;
                Items.RemoveAt(i);
                return result;
            }

            return null;
        }

        public static String RemoveBan(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                String result = Items[index].Name;
                Items.RemoveAt(index);
                return result;
            }

            return null;
        }

        public static void ListBans(UserObject userobj)
        {
            int i = 0;
            Items.ForEach(x => userobj.SendPacket(AresTCPPackets.NoSuch((i++) + " - " + x.Name + " [" + x.ExternalIP + "]")));

            if (i == 0)
                userobj.SendPacket(AresTCPPackets.NoSuch("no banned users"));
        }

        public static String LoadRecords()
        {
            return null;
        }

        private static void UpdateRecords()
        {

        }
    }
}
