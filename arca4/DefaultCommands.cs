﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.Protocol;

namespace arca4
{
    class DefaultCommands
    {
        public static void LoadScript(UserObject userobj, String args)
        {
            if (userobj.Level < Settings.ScriptLevel)
                return;
        }

        public static void KillScript(UserObject userobj, String args)
        {
            if (userobj.Level < Settings.ScriptLevel)
                return;
        }

        public static void ListScripts(UserObject userobj)
        {
            if (userobj.Level < Settings.ScriptLevel)
                return;
        }

        public static void Ban(UserObject userobj, UserObject target)
        {
            if (userobj.Level < Settings.BanLevel || target == null)
                return;

            if (target.Level < userobj.Level)
            {
                target.Expired = true;
                Bans.AddBan(target);
                DebugLog.WriteLine(target.Name + " banned by " + userobj.Name);
            }
        }

        public static void Unban(UserObject userobj, String args)
        {
            if (userobj.Level < Settings.BanLevel)
                return;

            int index;
            String name = null;

            if (int.TryParse(args, out index))
                name = Bans.RemoveBan(index);
            else if (args.Length > 2)
                if ((args.StartsWith("\"") && args.EndsWith("\"")) ||
                    (args.StartsWith("'") && args.EndsWith("'")))
                    name = Bans.RemoveBan(args.Substring(1, args.Length - 2));

            if (name != null)
                DebugLog.WriteLine(name + " unbanned by " + userobj.Name);
        }

        public static void ListBans(UserObject userobj)
        {
            if (userobj.Level < Settings.BanLevel)
                return;

            Bans.ListBans(userobj);
        }

        public static void Muzzle(UserObject userobj, UserObject target)
        {
            if (userobj.Level < Settings.MuzzleLevel || target == null)
                return;

            if (target.Level < userobj.Level && !target.Muzzled)
            {
                target.Muzzled = true;
                Muzzles.AddMuzzle(target);
                DebugLog.WriteLine(target.Name + " muzzled by " + userobj.Name);
            }
        }

        public static void Unmuzzle(UserObject userobj, UserObject target)
        {
            if (userobj.Level < Settings.MuzzleLevel || target == null)
                return;

            if (target.Muzzled)
            {
                target.Muzzled = false;
                Muzzles.RemoveMuzzle(target);
                DebugLog.WriteLine(target.Name + " unmuzzled by " + userobj.Name);
            }
        }

        public static void Kill(UserObject userobj, UserObject target)
        {
            if (userobj.Level < Settings.KillLevel || target == null)
                return;

            if (target.Level < userobj.Level)
            {
                target.Expired = true;
                DebugLog.WriteLine(target.Name + " killed by " + userobj.Name);
            }
        }

        public static void SetUserLevel(UserObject userobj, UserObject target, String args)
        {
            if (userobj.Level < 3 || target == null)
                return;

            byte b = 0;
            
            if (byte.TryParse(args, out b))
            {
                byte old_level = target.Level;
                target.Level = b;

                if (target.Registered)
                    UserAccounts.UpdateAdminLevel(target);

                DebugLog.WriteLine(target.Name + " set admin level " + b + " by " + userobj.Name);

                if (b > old_level)
                    ServerEvents.OnLoginGranted(target);
            }
        }

        public static void Info(UserObject userobj)
        {
            UserPool.Users.ForEach(x => { if (x.LoggedIn) userobj.SendPacket(AresTCPPackets.NoSuch(x.ID + " - " + x.Name + " -> " + x.Vroom)); });
        }
    }
}