using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;
using Ares.Protocol;

namespace arca4
{
    class CustomPacketProcessor
    {
        public static void Eval(UserObject userobj, ProtoMessage msg, AresTCPPacketReader packet)
        {
            switch (msg)
            {
                case ProtoMessage.MSG_CHAT_CLIENT_CUSTOM_ADD_TAGS:
                    ProcessAddCustomTag(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_CUSTOM_REM_TAGS:
                    ProcessRemCustomTag(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_CUSTOM_FONT:
                    ProcessCustomFont(userobj, packet);
                    break;
            }
        }

        private static void ProcessAddCustomTag(UserObject userobj, AresTCPPacketReader packet)
        {
            while (packet.Remaining > 0)
                userobj.CustomTags.Add(packet.ReadString());
        }

        private static void ProcessRemCustomTag(UserObject userobj, AresTCPPacketReader packet)
        {
            while (packet.Remaining > 0)
            {
                String str = packet.ReadString();
                userobj.CustomTags.Remove(str);
            }
        }

        private static void ProcessCustomFont(UserObject userobj, AresTCPPacketReader packet)
        {
            if (packet.Remaining == 2)
            {
                userobj.Font = null;
                UserPool.BroadcastToVroom(userobj.Vroom, CustomPackets.CustomFontDefault(userobj));
            }
            else
            {
                userobj.Font = new Font(packet.ReadByte(), packet.ReadString(), packet.ReadByte(), packet.ReadByte());
                UserPool.BroadcastToVroom(userobj.Vroom, CustomPackets.CustomFont(userobj));
            }
        }


    }
}
