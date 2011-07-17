using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace arca4
{
    class Settings
    {
        private static IPAddress localip;

        public static IPAddress UDPAddress
        {
            get { return IPAddress.Any; }
        }

        public static IPAddress ExternalIP
        {
            get { return IPAddress.Loopback; }
        }

        public static IPAddress LocalIP
        {
            get
            {
                if (localip == null)
                {
                    foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localip = ip;
                            break;
                        }

                    if (localip == null)
                        localip = IPAddress.Loopback;
                }

                return localip;
            }
        }

        public static ushort Port
        {
            get { return 22454; }
        }

        public static String Name
        {
            get { return "test"; }
        }

        public static String BotName
        {
            get { return "Arca4"; }
        }

        public static String Version
        {
            get { return "Arca4 project http://arca4.codeplex.com/"; }
        }

        public static byte Language
        {
            get { return 10; }
        }

        public static String Topic
        {
            get { return "Arca4 project test room"; }
        }
    }
}
