using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;

namespace arca4
{
    class AresTCPPacketProcessor
    {
        public static void Evaluate(UserObject userobj, ProtoMessage msg, AresTCPPacketReader packet, uint time)
        {
            if (!userobj.LoggedIn)
                if (msg > ProtoMessage.MSG_CHAT_CLIENT_LOGIN)
                    throw new Exception();

            switch (msg)
            {
                case ProtoMessage.MSG_CHAT_CLIENT_LOGIN:
                    ProcessLogin(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_UPDATE_STATUS:
                    userobj.Pinged(time);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_AVATAR:
                    userobj.Avatar = packet.ReadBytes();
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_PERSONAL_MESSAGE:
                    userobj.PersonalMessage = packet.ReadString();
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

            userobj.LoggedIn = true;
            userobj.SendPacket(AresTCPPackets.LoginAck(userobj));
            userobj.SendPacket(AresTCPPackets.MyFeatures(userobj));
            userobj.SendPacket(AresTCPPackets.TopicFirst());
            UserPool.SendUserList(userobj);
            userobj.SendPacket(AresTCPPackets.OpChange(userobj));
            ServerEvents.OnJoin(userobj);
        }

    }
}
