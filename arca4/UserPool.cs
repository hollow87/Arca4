using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arca4
{
    class UserPool
    {
        public static List<UserObject> Users;

        public static void Init()
        {
            Users = new List<UserObject>();
        }

        public static void SetID(UserObject userobj)
        {
            userobj.ID = -1;
            int id = 0;

            while (true)
            {
                if (Users.Find(x => x.ID == id) != null)
                    id++;
                else
                {
                    userobj.ID = id;
                    break;
                }
            }

            if (Users.Count > 1)
                Users.Sort((x, y) => x.ID - y.ID);
        }

    }
}
