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
        private class BannedItem
        {
            public String Name { get; private set; }
            public IPAddress ExternalIP { get; private set; }
            public IPAddress LocalIP { get; private set; }
            public ushort Port { get; private set; }
            public Guid Guid { get; private set; }
            public String Version { get; private set; }

            public BannedItem(UserObject userobj)
            {
                this.Name = userobj.Name;
                this.ExternalIP = userobj.ExternalIP;
                this.LocalIP = userobj.LocalIP;
                this.Port = userobj.Port;
                this.Guid = userobj.Guid;
                this.Version = userobj.Version;
            }
        }

        private static List<BannedItem> Items = new List<BannedItem>();

        public static bool IsBanned(UserObject userobj)
        {
            return Items.Find(x => x.ExternalIP.Equals(userobj.ExternalIP) || x.Guid.Equals(userobj.Guid)) != null;
        }

        public static void AddBan(UserObject userobj)
        {
            Items.Add(new BannedItem(userobj));
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
