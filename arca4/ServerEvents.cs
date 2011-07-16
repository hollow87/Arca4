using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return text;
        }

        public static void OnTextAfter(UserObject userobj, String text)
        {

        }

        public static String OnEmoteBefore(UserObject userobj, String text)
        {
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

    }
}
