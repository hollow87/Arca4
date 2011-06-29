using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;

namespace arca4
{
    class AresTCPPacketProcessor
    {
        public static void Evaluate(UserObject userobj, ProtoMessage msg, AresTCPPacketReader packet)
        {
            switch (msg)
            {
                case ProtoMessage.MSG_CHAT_CLIENT_LOGIN:
                    ProcessLogin(userobj, packet);
                    break;
            }
        }

        private static void ProcessLogin(UserObject userobj, AresTCPPacketReader packet)
        {
            if (userobj.LoggedIn)
                throw new Exception();

            userobj.PopulateCredentials(packet);

            if (!ServerEvents.OnJoinCheck(userobj))
            {
                ServerEvents.OnRejected(userobj);
                userobj.Expired = true;
                return;
            }

            ServerEvents.OnJoin(userobj);
        }

    }
}
