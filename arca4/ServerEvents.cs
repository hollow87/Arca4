using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.Protocol;

namespace arca4
{
    class ServerEvents
    {
        public static void OnTimer()
        {

        }

        public static bool OnJoinCheck(UserObject userobj)
        {
            return true;
        }

        public static void OnRejected(UserObject userobj, RejectionType e)
        {

        }

        public static void OnMOTD(UserObject userobj)
        {

        }

        public static void OnJoin(UserObject userobj)
        {
            
        }

        public static void OnPart(UserObject userobj)
        {
            
        }

        public static bool OnAvatarReceived(UserObject userobj)
        {
            return true;
        }

        public static bool OnPersonalMessageReceived(UserObject userobj)
        {
            return true;
        }

        public static String OnTextBefore(UserObject userobj, String text)
        {
            if (userobj.Muzzled)
            {
                userobj.SendPacket(AresTCPPackets.NoSuch("You are muzzled"));
                return String.Empty;
            }

            return text;
        }

        public static void OnTextAfter(UserObject userobj, String text)
        {

        }

        public static String OnEmoteBefore(UserObject userobj, String text)
        {
            if (userobj.Muzzled)
            {
                userobj.SendPacket(AresTCPPackets.NoSuch("You are muzzled"));
                return String.Empty;
            }

            return text;
        }

        public static void OnEmoteAfter(UserObject userobj, String text)
        {

        }

        public static String OnPM(UserObject userobj, UserObject target, String text)
        {
            return text;
        }

        public static void OnBotPM(UserObject userobj, String text)
        {

        }

        public static void OnCommand(UserObject userobj, String command, UserObject target, String args)
        {
            if (command.StartsWith("loadscript "))
            {
                DefaultCommands.LoadScript(userobj, command.Substring(11));
                return;
            }
            else if (command.StartsWith("killscript "))
            {
                DefaultCommands.KillScript(userobj, command.Substring(11));
                return;
            }
            else if (command == "listscripts")
            {
                DefaultCommands.ListScripts(userobj);
                return;
            }

            // add scripting here so that remaining default commands can be overridden

            if (command.StartsWith("ban "))
                DefaultCommands.Ban(userobj, target);
            else if (command.StartsWith("unban "))
                DefaultCommands.Unban(userobj, command.Substring(6));
            else if (command == "listbans")
                DefaultCommands.ListBans(userobj);
            else if (command.StartsWith("muzzle "))
                DefaultCommands.Muzzle(userobj, target);
            else if (command.StartsWith("unmuzzle "))
                DefaultCommands.Unmuzzle(userobj, target);
            else if (command.StartsWith("kill ") || command.StartsWith("kick "))
                DefaultCommands.Kill(userobj, target);
            else if (command.StartsWith("setuserlevel "))
                DefaultCommands.SetUserLevel(userobj, target, args);
            else if (command == "info")
                DefaultCommands.Info(userobj);
        }

        public static void OnHelp(UserObject userobj)
        {
            userobj.SendPacket(AresTCPPackets.NoSuch("#register <password>"));
            userobj.SendPacket(AresTCPPackets.NoSuch("#unregister <password>"));
            userobj.SendPacket(AresTCPPackets.NoSuch("#login <password>"));
            userobj.SendPacket(AresTCPPackets.NoSuch("#info"));

            if (userobj.Level >= Settings.MuzzleLevel)
            {
                userobj.SendPacket(AresTCPPackets.NoSuch("#muzzle <user>"));
                userobj.SendPacket(AresTCPPackets.NoSuch("#unmuzzle <user>"));
            }

            if (userobj.Level >= Settings.KillLevel)
                userobj.SendPacket(AresTCPPackets.NoSuch("#kill <user>"));

            if (userobj.Level >= Settings.BanLevel)
            {
                userobj.SendPacket(AresTCPPackets.NoSuch("#ban <user>"));
                userobj.SendPacket(AresTCPPackets.NoSuch("#unban <user>"));
                userobj.SendPacket(AresTCPPackets.NoSuch("#listbans"));
            }

            if (userobj.Level == 3)
                userobj.SendPacket(AresTCPPackets.NoSuch("#setuserlevel <user> <level>"));

            if (userobj.Level >= Settings.ScriptLevel)
            {
                userobj.SendPacket(AresTCPPackets.NoSuch("#loadscript <filename>"));
                userobj.SendPacket(AresTCPPackets.NoSuch("#killscript <filename>"));
                userobj.SendPacket(AresTCPPackets.NoSuch("#listscripts"));
            }
        }

        public static bool OnVroomJoinCheck(UserObject userobj, ushort vroom)
        {
            return true;
        }

        public static void OnVroomJoin(UserObject userobj)
        {

        }

        public static void OnVroomPart(UserObject userobj)
        {

        }

        public static void OnFileReceived(UserObject userobj, String filename, String title)
        {
            
        }

        public static void OnLoginGranted(UserObject userobj)
        {

        }

        public static void OnInvalidPassword(UserObject userobj)
        {
            
        }

    }
}
