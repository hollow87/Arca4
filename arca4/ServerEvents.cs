using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using arca4.Scripting;
using arca4.Scripting.JS;
using Ares.Protocol;
using smnetjs;
namespace arca4
{
    class ServerEvents
    {
		private static Dictionary<EventType, Dictionary<string, SMFunction>> events;

		static ServerEvents()
		{
			events = new Dictionary<EventType, Dictionary<string, SMFunction>>();
			foreach(string eventName in Enum.GetNames(typeof(EventType)))
			{
				events[(EventType)Enum.Parse(typeof(EventType), eventName)] =
					new Dictionary<string, SMFunction>();
			}

		}

		public static void RegisterEvent(string script, EventType @event, SMFunction method)
		{
			if (!events.ContainsKey(@event))
			{
				events[@event] = new Dictionary<string, SMFunction>();
			}

			events[@event][script] = method;
		}

		public static void InvokeEvent(EventType @event, params object[] args)
		{
			foreach (var method in events[@event].Values)
			{
				method.Invoke(args);
			}
		}
		public static T InvokeEvent<T>(EventType @event, params object[] args)
		{
			T ret = default(T);
			
			if (ret is bool)
			{
				ret = (T)(object)true;
			}
			
			foreach (var method in events[@event].Values)
			{
				if (ret is bool)
				{
					

					ret = (T)(object)(Convert.ToBoolean(ret) & Convert.ToBoolean(method.Invoke(args)));
					
					return ret;
				}

				ret = (T)(object)(Convert.ToString(method.Invoke(args)));

			}

			return ret;
		}

        public static void OnTimer()
        {
			InvokeEvent(EventType.OnTimer);
        }

        public static bool OnFlood(UserObject userobj)
        {
			return InvokeEvent<bool>(EventType.OnFlood, userobj.ScriptUserObj);
        }

        public static bool OnJoinCheck(UserObject userobj)
        {
			return InvokeEvent<bool>(EventType.OnJoinCheck, userobj.ScriptUserObj);
        }

        public static void OnRejected(UserObject userobj, RejectionType e)
        {
			InvokeEvent(EventType.OnRejected, userobj.ScriptUserObj, e);
        }

        public static void OnJoin(UserObject userobj)
        {
			InvokeEvent(EventType.OnJoin, userobj.ScriptUserObj);
        }

        public static void OnPart(UserObject userobj)
        {
			InvokeEvent(EventType.OnPart, userobj.ScriptUserObj);
		}

        public static bool OnAvatarReceived(UserObject userobj)
        {
			return InvokeEvent<bool>(EventType.OnAvatarReceived, userobj.ScriptUserObj);
        }

        public static bool OnPersonalMessageReceived(UserObject userobj)
        {
			return InvokeEvent<bool>(EventType.OnPersonalMessageReceived, userobj.ScriptUserObj);
        }

        public static String OnTextBefore(UserObject userobj, String text)
        {
            if (userobj.Muzzled)
            {
                userobj.SendPacket(AresTCPPackets.NoSuch("You are muzzled"));
                return String.Empty;
            }


			string ret = InvokeEvent<string>(EventType.OnTextBefore, userobj.ScriptUserObj, text);

			return ret == null ? text : ret;
        }

        public static void OnTextAfter(UserObject userobj, String text)
        {

			InvokeEvent(EventType.OnTextAfter, userobj.ScriptUserObj, text);
        }

        public static String OnEmoteBefore(UserObject userobj, String text)
        {
            if (userobj.Muzzled)
            {
                userobj.SendPacket(AresTCPPackets.NoSuch("You are muzzled"));
                return String.Empty;
            }

			string ret = InvokeEvent<string>(EventType.OnEmoteBefore, userobj.ScriptUserObj, text);

			return ret == null ? text : ret;
        }

        public static void OnEmoteAfter(UserObject userobj, String text)
        {
			InvokeEvent(EventType.OnEmoteAfter, userobj.ScriptUserObj, text);
        }

        public static String OnPM(UserObject userobj, UserObject target, String text)
        {
			string ret = InvokeEvent<string>(EventType.OnPM, userobj.ScriptUserObj, text);

			return ret == null ? text : ret;
        }

        public static void OnBotPM(UserObject userobj, String text)
        {
			InvokeEvent(EventType.OnBotPM, userobj.ScriptUserObj, text);
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

			InvokeEvent(EventType.OnCommand, userobj.ScriptUserObj, command, target.ScriptUserObj, args, bot);

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
			/*  NOT IMPLEMNTING YET IN SCRIPT
			 * TODO: Need a better way to implement the core functions if some of these commands can be overridden in script
			
			*/

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
			return InvokeEvent<bool>(EventType.OnVroomJoinCheck, userobj.ScriptUserObj, vroom);
        }

        public static void OnVroomJoin(UserObject userobj)
        {
			InvokeEvent(EventType.OnVroomJoin, userobj.ScriptUserObj);
        }

        public static void OnVroomPart(UserObject userobj)
        {
			InvokeEvent(EventType.OnVroomPart, userobj.ScriptUserObj);
        }

        public static void OnFileReceived(UserObject userobj, String filename, String title)
        {
			InvokeEvent(EventType.OnFileReceived, userobj.ScriptUserObj, filename, title);
        }

        public static void OnLoginGranted(UserObject userobj)
        {
			InvokeEvent(EventType.OnLoginGranted, userobj.ScriptUserObj);
        }

        public static void OnInvalidPassword(UserObject userobj)
        {
			InvokeEvent(EventType.OnInvalidPassword, userobj.ScriptUserObj);
        }

    }
}
