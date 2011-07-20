using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;
using Ares.Protocol;

namespace arca4
{
    class MOTD
    {
        private static List<String> motd;

        public static void SendMOTD(UserObject userobj)
        {
            List<byte> buf = new List<byte>();

            motd.ForEach(x =>
            {
                buf.AddRange(AresTCPPackets.NoSuch(x.Replace("+n", userobj.Name)));

                if (buf.Count > 800)
                {
                    userobj.SendPacket(AresTCPPackets.ClientCompressed(buf.ToArray()));
                    buf.Clear();
                }
            });

            if (buf.Count > 0)
                userobj.SendPacket(AresTCPPackets.ClientCompressed(buf.ToArray()));
        }

        public static void LoadMOTD()
        {
            motd = new List<String>();
            /* we would load from file */
            motd.Add("Welcome to my room +n");
        }
    }
}
