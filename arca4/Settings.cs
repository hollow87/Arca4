using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;

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

        public static byte ScriptLevel
        {
            get { return 3; }
        }

        public static byte BanLevel
        {
            get { return 2; }
        }

        public static byte KillLevel
        {
            get { return 1; }
        }

        public static byte MuzzleLevel
        {
            get { return 1; }
        }

        private static object GetValue(String name)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\arca4");

            if (key == null)
                return null;

            object value = key.GetValue(name);
            key.Close();
            return value;
        }

        private static void SetValue(String name, String value, RegistryValueKind kind)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\arca4", true);

            if (key == null)
            {
                Registry.CurrentUser.CreateSubKey("Software\\arca4");
                key = Registry.CurrentUser.OpenSubKey("Software\\arca4", true);
            }

            key.SetValue(name, value, kind);
            key.Close();
        }
    }
}
