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

    }
}
