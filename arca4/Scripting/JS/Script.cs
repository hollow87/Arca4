using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using smnetjs;
namespace arca4.Scripting.JS
{
	class Script
	{
		[SMMethod(Script = true)]
		public static void Create(SMScript script, string name)
		{
			ScriptManager.Items.Add(name);
		}

		[SMMethod(Script = true)]
		public static string Eval(SMScript script, string name, string text)
		{
			return ScriptManager.Items.Find(name).Eval(text);
		}
	}
}
