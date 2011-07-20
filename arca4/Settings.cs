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
        private static String name;
        private static String topic;

        public static IPAddress UDPAddress
        {
            get
            {
                object result = GetValue("udp_ip");

                if (result == null)
                {
                    result = IPAddress.Any.GetAddressBytes();
                    SetValue<byte[]>("udp_ip", (byte[])result, RegistryValueKind.Binary);
                }

                return new IPAddress((byte[])result);
            }
        }

        public static IPAddress ExternalIP
        {
            get
            {
                object result = GetValue("external_ip");

                if (result == null)
                {
                    result = IPAddress.Loopback.GetAddressBytes();
                    SetValue<byte[]>("external_ip", (byte[])result, RegistryValueKind.Binary);
                }

                return new IPAddress((byte[])result);
            }
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
            get
            {
                object result = GetValue("port");

                if (result == null)
                {
                    result = (int)22454;
                    SetValue<int>("port", (int)result, RegistryValueKind.DWord);
                }

                int i = (int)result;
                return (ushort)i;
            }
        }

        public static String Name
        {
            get
            {
                if (!String.IsNullOrEmpty(name))
                    return name;

                object result = GetValue("name");

                if (result == null)
                {
                    result = Encoding.UTF8.GetBytes("test");
                    SetValue<byte[]>("name", (byte[])result, RegistryValueKind.Binary);
                }

                name = Encoding.UTF8.GetString((byte[])result);
                return name;
            }
        }

        public static String BotName
        {
            get
            {
                object result = GetValue("bot");

                if (result == null)
                {
                    result = Encoding.UTF8.GetBytes("Arca4");
                    SetValue<byte[]>("bot", (byte[])result, RegistryValueKind.Binary);
                }

                return Encoding.UTF8.GetString((byte[])result);
            }
        }

        public static String Version
        {
            get { return "Arca4 project http://arca4.codeplex.com/"; }
        }

        public static byte Language
        {
            get
            {
                object result = GetValue("language");

                if (result == null)
                {
                    result = (int)10;
                    SetValue<int>("language", (int)result, RegistryValueKind.DWord);
                }

                int i = (int)result;
                return (byte)i;
            }
        }

        public static String Topic
        {
            get
            {
                if (!String.IsNullOrEmpty(topic))
                    return topic;

                object result = GetValue("topic");

                if (result == null)
                {
                    result = Encoding.UTF8.GetBytes("Arca4 project test room");
                    SetValue<byte[]>("topic", (byte[])result, RegistryValueKind.Binary);
                }

                topic = Encoding.UTF8.GetString((byte[])result);
                return topic;
            }
            set
            {
                topic = value;
                SetValue<byte[]>("topic", Encoding.UTF8.GetBytes(topic), RegistryValueKind.Binary);
            }
        }

        public static byte ScriptLevel
        {
            get
            {
                object result = GetValue("script_level");

                if (result == null)
                {
                    result = (int)3;
                    SetValue<int>("script_level", (int)result, RegistryValueKind.DWord);
                }

                int i = (int)result;
                return (byte)i;
            }
        }

        public static byte BanLevel
        {
            get
            {
                object result = GetValue("ban_level");

                if (result == null)
                {
                    result = (int)2;
                    SetValue<int>("ban_level", (int)result, RegistryValueKind.DWord);
                }

                int i = (int)result;
                return (byte)i;
            }
        }

        public static byte KillLevel
        {
            get
            {
                object result = GetValue("kill_level");

                if (result == null)
                {
                    result = (int)1;
                    SetValue<int>("kill_level", (int)result, RegistryValueKind.DWord);
                }

                int i = (int)result;
                return (byte)i;
            }
        }

        public static byte MuzzleLevel
        {
            get
            {
                object result = GetValue("muzzle_level");

                if (result == null)
                {
                    result = (int)1;
                    SetValue<int>("muzzle_level", (int)result, RegistryValueKind.DWord);
                }

                int i = (int)result;
                return (byte)i;
            }
        }

        public static bool CanRegister
        {
            get
            {
                object result = GetValue("accepting_registration");

                if (result == null)
                {
                    result = (int)1;
                    SetValue<int>("accepting_registration", (int)result, RegistryValueKind.DWord);
                }

                int i = (int)result;
                return i == 1;
            }
        }

        public static bool GeneralCommands
        {
            get
            {
                object result = GetValue("general_commands");

                if (result == null)
                {
                    result = (int)0;
                    SetValue<int>("general_commands", (int)result, RegistryValueKind.DWord);
                }

                int i = (int)result;
                return i == 1;
            }
            set
            {
                int result = (value ? 1 : 0);
                SetValue<int>("general_commands", (int)result, RegistryValueKind.DWord);
            }
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

        private static void SetValue<T>(String name, T value, RegistryValueKind kind)
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
