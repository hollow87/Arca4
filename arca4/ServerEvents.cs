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

        public static bool OnFlood(UserObject userobj)
        {
            return true;
        }

        public static bool OnJoinCheck(UserObject userobj)
        {
            return true;
        }

        public static void OnRejected(UserObject userobj, RejectionType e)
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

        public static void OnCommand(UserObject userobj, String command, UserObject target, String args, Boolean bot)
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

        private static String[] PUBLIC_COMMANDS = { "#register <password>", "#unregister <password>", "login <password>", "#info" };

        public static void OnHelp(UserObject userobj, Boolean bot)
        {
            foreach (String command in PUBLIC_COMMANDS)
            {
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, command) : AresTCPPackets.NoSuch(command)));
            }

            if (userobj.Level >= Settings.MuzzleLevel)
            {
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#muzzle <user>") : AresTCPPackets.NoSuch("#muzzle <user>")));
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#muzzle <user>") : AresTCPPackets.NoSuch("#muzzle <user>")));
            }

            if (userobj.Level >= Settings.KillLevel)
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#kill <user>") : AresTCPPackets.NoSuch("#kill <user>")));

            if (userobj.Level >= Settings.BanLevel)
            {
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#ban <user>") : AresTCPPackets.NoSuch("#ban <user>")));
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#unban <user>") : AresTCPPackets.NoSuch("#unban <user>")));
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#listbans") : AresTCPPackets.NoSuch("#listbans")));
            }

            if (userobj.Level == 3)
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#setuserlevel <user> <level>") : AresTCPPackets.NoSuch("#setuserlevel <user> <level>")));

            if (userobj.Level >= Settings.ScriptLevel)
            {
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#loadscript <filename>") : AresTCPPackets.NoSuch("#loadscript <filename>")));
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#killscript <filename>") : AresTCPPackets.NoSuch("#killscript <filename>")));
                userobj.SendPacket((bot ? AresTCPPackets.Private(Settings.BotName, "#listscripts") : AresTCPPackets.NoSuch("#listscripts")));
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
