using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arca4
{
    class ServerEvents
    {
        public static bool OnJoinCheck(UserObject userobj)
        {
            UserPool.BroadcastToVroom(userobj.Vroom, AresTCPPackets.Join(userobj));
            return true;
        }

        public static void OnRejected(UserObject userobj)
        {

        }

        public static void OnJoin(UserObject userobj)
        {

        }

        public static void OnPart(UserObject userobj)
        {
            UserPool.BroadcastToVroom(userobj.Vroom, AresTCPPackets.Part(userobj));
        }

    }
}
