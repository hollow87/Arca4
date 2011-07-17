using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace arca4
{
    class Helpers
    {
        public static CommandObject TextToCommand(UserObject userobj, String text)
        {
            CommandObject cmd = new CommandObject();
            String str = text;
            int i = str.IndexOf(" ");

            if (i > -1)
            {
                str = str.Substring(i + 1);

                if (str.Length > 0)
                {
                    cmd.target = UserPool.Users.Find(x => x.LoggedIn && x.Name == str);

                    if (cmd.target != null) // right click command
                        return cmd;
                    else if (str[0] == '\'' || str[0] == '"')
                    {
                        char c = str[0];
                        str = str.Substring(1);
                        i = str.IndexOf(c);

                        if (i > 0)
                        {
                            cmd.target = UserPool.Users.Find(x => x.LoggedIn && x.Name.StartsWith(str.Substring(0, i)));

                            if (cmd.target != null)
                            {
                                str = str.Substring(i + 1);

                                if (str.Length > 1)
                                    cmd.args = str.Substring(1);

                                return cmd;
                            }
                        }
                    }
                    else
                    {
                        i = str.IndexOf(" ");

                        if (i > 0)
                        {
                            if (int.TryParse(str.Substring(0, i), out i))
                                if (i > -1)
                                {
                                    cmd.target = UserPool.Users.Find(x => x.LoggedIn && x.ID == i);

                                    if (cmd.target != null)
                                    {
                                        cmd.args = str.Substring(str.IndexOf(" ") + 1);
                                        return cmd;
                                    }
                                }
                        }
                        else if (int.TryParse(str, out i))
                            if (i > -1)
                            {
                                cmd.target = UserPool.Users.Find(x => x.LoggedIn && x.ID == i);
                                return cmd;
                            }
                    }
                }
            }

            cmd.target = null;
            cmd.args = String.Empty;
            return cmd;
        }

        public static bool IsLocalHost(IPAddress ip)
        {
            byte[] buff = ip.GetAddressBytes();

            switch (buff[0])
            {
                case 192:
                    if (buff[1] == 168)
                        return true;
                    break;

                case 127:
                    return true;

                case 10:
                    if (buff[1] == 0)
                        return true;
                    break;
            }

            return ip.Equals(Settings.ExternalIP);
        }
    }
}
