using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace arca4
{
    class UserAccounts
    {
        public static void Login(UserObject userobj, String password)
        {

        }

        public static void Login(UserObject userobj, byte[] password)
        {

        }

        public static void Register(UserObject userobj, String password)
        {
            if (password.Length < 2)
                return;

            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\arca4");

            if (key == null)
                Registry.CurrentUser.CreateSubKey("Software\\arca4");
            else key.Close();

            key = Registry.CurrentUser.OpenSubKey("Software\\arca4\\accounts", true);

            if (key == null)
            {
                Registry.CurrentUser.CreateSubKey("Software\\arca4\\accounts");
                key = Registry.CurrentUser.OpenSubKey("Software\\arca4\\accounts", true);
            }

            String value_name = String.Empty;

            foreach (byte b in userobj.Guid.ToByteArray())
                value_name += String.Format("{0:X2}", b);

            if (key.GetValue(value_name) == null)
            {
                List<byte> list = new List<byte>();
                list.AddRange(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
                list.Add(userobj.Level);
                list.AddRange(Encoding.UTF8.GetBytes(userobj.Name));

                key.SetValue(value_name, list.ToArray(), RegistryValueKind.Binary);
                userobj.SendPacket(AresTCPPackets.NoSuch("You are registered using password: " + password));
                DebugLog.WriteLine(userobj.Name + " has registered to this room");
            }
            else userobj.SendPacket(AresTCPPackets.NoSuch("Unregister current password first"));

            key.Close();
        }

        public static void Unregister(UserObject userobj, String password)
        {
            if (password.Length < 2)
                return;

            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\arca4");

            if (key == null)
                return;

            key.Close();
            key = Registry.CurrentUser.OpenSubKey("Software\\arca4\\accounts");

            if (key == null)
                return;

            String value_name = String.Empty;

            foreach (byte b in userobj.Guid.ToByteArray())
                value_name += String.Format("{0:X2}", b);

            if (key.GetValue(value_name) == null)
                return;

            List<byte> list = new List<byte>((byte[])key.GetValue(value_name));
            byte[] current_password = list.GetRange(0, 20).ToArray();
            list.RemoveRange(0, 21);
            String owner = Encoding.UTF8.GetString(list.ToArray());
            byte[] reported_password = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            key.Close();

            if (reported_password.SequenceEqual(current_password))
            {
                key = Registry.CurrentUser.OpenSubKey("Software\\arca4\\accounts", true);
                key.DeleteValue(value_name);
                key.Close();
                userobj.SendPacket(AresTCPPackets.NoSuch("You are unregistered"));
                DebugLog.WriteLine(userobj.Name + " has unregistered from this room (account originally created by " + owner + ")");
            }
            else
            {
                ServerEvents.OnInvalidPassword(userobj);
                DebugLog.WriteLine(userobj.Name + " failed to unregister using an invalid password (account originally created by " + owner + ")");
            }
        }
    }
}
