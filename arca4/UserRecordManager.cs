using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace arca4
{
    class UserRecordManager
    {
        private static List<UserRecordItem> items;

        public static void Init()
        {
            items = new List<UserRecordItem>();
        }

        public static void AddItem(UserObject userobj)
        {
            items.Add(new UserRecordItem(userobj));
        }

        public static bool IsJoinFlooding(UserObject userobj, uint time)
        {
            return items.FindAll(x => x.ExternalIP.Equals(userobj.ExternalIP) && (x.Time + 15) > time).Count > 0;
        }
    }

    class UserRecordItem
    {
        public String Name { get; private set; }
        public String Version { get; private set; }
        public Guid Guid { get; private set; }
        public IPAddress ExternalIP { get; private set; }
        public IPAddress LocalIP { get; private set; }
        public ushort Port { get; private set; }
        public uint Time { get; private set; }

        public UserRecordItem(UserObject userobj)
        {
            this.Name = userobj.Name;
            this.Version = userobj.Version;
            this.Guid = userobj.Guid;
            this.ExternalIP = userobj.ExternalIP;
            this.LocalIP = userobj.LocalIP;
            this.Port = userobj.Port;
            this.Time = userobj.Cookie;
        }
    }
}
