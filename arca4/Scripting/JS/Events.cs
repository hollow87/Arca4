using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using smnetjs;

namespace arca4.Scripting.JS
{
	class Events
	{
		
		[SMMethod(Script = true)]
		public static void Register(SMScript script, EventType @event, SMFunction method)
		{
			
			ScriptObj scriptObj = ScriptManager.Items.Find(script.Name);
			ServerEvents.RegisterEvent(scriptObj.ScriptName, @event, method);
		}
	}
}
