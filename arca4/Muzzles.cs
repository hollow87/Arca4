using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using Ares.Protocol;

namespace arca4
{
    class Muzzles
    {
        private class MuzzledItem
        {
            public String Name { get; private set; }
            public IPAddress ExternalIP { get; private set; }
            public ushort Port { get; private set; }
            public Guid Guid { get; private set; }

            public MuzzledItem(UserObject userobj)
            {
                this.Name = userobj.Name;
                this.ExternalIP = userobj.ExternalIP;
                this.Port = userobj.Port;
                this.Guid = userobj.Guid;
            }
        }

        private static List<MuzzledItem> Items = new List<MuzzledItem>();

        public static bool IsMuzzled(UserObject userobj)
        {
            return Items.Find(x => x.ExternalIP.Equals(userobj.ExternalIP) || x.Guid.Equals(userobj.Guid)) != null;
        }

        public static void AddMuzzle(UserObject userobj)
        {
            UserPool.Users.FindAll(x => x.LoggedIn && (x.ExternalIP.Equals(userobj.ExternalIP) || x.Guid.Equals(userobj.Guid))).ForEach(x =>
            {
                x.SendPacket(AresTCPPackets.NoSuch("You are muzzled"));
                x.Muzzled = true;
            });

            Items.Add(new MuzzledItem(userobj));
        }

        public static void RemoveMuzzle(UserObject userobj)
        {
            UserPool.Users.FindAll(x => x.LoggedIn && (x.ExternalIP.Equals(userobj.ExternalIP) || x.Guid.Equals(userobj.Guid))).ForEach(x =>
            {
                x.SendPacket(AresTCPPackets.NoSuch("You are unmuzzled"));
                x.Muzzled = false;
            });

            Items.RemoveAll(x => x.Guid.Equals(userobj.Guid) || x.ExternalIP.Equals(userobj.ExternalIP));
        }

        public static void LoadRecords()
        {

        }

        private static void UpdateRecords()
        {
            
        }
    }
}
