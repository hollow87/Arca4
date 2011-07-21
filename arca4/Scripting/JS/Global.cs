using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.Protocol;
using smnetjs;

namespace arca4.Scripting.JS
{
	sealed class Global
	{

		[SMMethod(Name = "print")]
		public static void Print(string obj)
		{
			UserPool.Broadcast(AresTCPPackets.NoSuch(obj));
		}

		[SMMethod(Name = "print")]
		public static void Print(ushort id, string obj)
		{
			UserPool.BroadcastToVroom(id, AresTCPPackets.NoSuch(obj)); ;
		}

		[SMMethod(Name = "user")]
		public static UserObj User(int id)
		{
			return UserPool.Users[id].ScriptUserObj;
		}

		[SMMethod(Name = "user")]
		public static UserObj User(string name)
		{

			return new UserObj(UserPool.Users.Where(c => c.OrgName == name).FirstOrDefault());
		}
	}
}
