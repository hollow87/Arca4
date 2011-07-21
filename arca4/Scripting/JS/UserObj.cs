using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using smnetjs;
namespace arca4.Scripting.JS
{
	[SMEmbedded(Name = "Arca4_User")]
	class UserObj
	{
		
		private UserObject userobj;

		[SMIgnore]
		public UserObj(UserObject userobj)
		{
			this.userobj = userobj;
		}

		public bool CanScript
		{
			get
			{
				return (userobj.Level >= Settings.ScriptLevel);
			}
		}
		
		public string Guid
		{
			get
			{
				
				return Convert.ToBase64String(SHA1.Create().ComputeHash(userobj.Guid.ToByteArray())) ;
			}
		}

		public byte Age
		{
			get { return userobj.Age; }
		}

		public bool CanBrowse
		{
			get { return userobj.CanBrowse; }
		}

		public byte Country
		{
			get { return userobj.Country; }
		}

		public byte CurrentQueued
		{
			get { return userobj.CurrentQueued; }
		}

		public byte CurrentUploads
		{
			get { return userobj.CurrentUploads; }
		}

		public void Disconnect()
		{
			userobj.Disconnect();
		}

		public string ExternalIP
		{
			get { return userobj.ExternalIP.ToString(); }
		}

		public bool FastPing
		{
			get { return userobj.FastPing; }
		}

		public ushort FileCount
		{
			get { return userobj.FileCount; }
		}

		public int ID
		{
			get { return userobj.ID; }
		}

		public byte Level
		{
			get { return userobj.Level; }
			
		}

		public string LocalIP
		{
			get { return userobj.LocalIP.ToString(); }
		}

		public string Location
		{
			get { return userobj.Location; }
		}

		public bool LoggedIn
		{
			get { return userobj.LoggedIn; }
		}

		public byte MaxUploads
		{
			get { return userobj.MaxUploads; }
		}

		public bool Muzzled
		{
			get
			{
				return userobj.Muzzled;
			}
			set
			{
				userobj.Muzzled = value;
			}
		}

		public string Name
		{
			get
			{
				return userobj.Name;
			}
			set
			{
				userobj.Name = value;
			}
		}

		public string NodeIP
		{
			get { return userobj.NodeIP.ToString(); }
		}

		public ushort NodePort
		{
			get { return userobj.NodePort; }
		}

		public string OrgName
		{
			get { return userobj.OrgName; }
		}

		public string PersonalMessage
		{
			get { return userobj.PersonalMessage; }
			
		}

		public ushort Port
		{
			get { return userobj.Port; }
		}

		public bool Registered
		{
			get { return userobj.Registered; }
		}

		public byte Sex
		{
			get { return userobj.Sex; }
		}

		public string Version
		{
			get { return userobj.Version; }
		}

		public ushort Vroom
		{
			get
			{
				return userobj.Vroom;
			}
			set
			{
				userobj.Vroom = value;
			}
		}
	}
}
