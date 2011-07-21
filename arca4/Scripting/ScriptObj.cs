using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.Protocol;
using smnetjs;

namespace arca4.Scripting
{
	sealed class ScriptObj
	{

		public SMScript Script { get; private set; }

		private bool isDebug = false;
		
		private string fileName = null;
		private string fullFilePath = null;


		public string ScriptName { get; set; }
		public string ScriptText { get; private set; }


		public ScriptObj(string scriptName, string scriptText)
		{
			ScriptName = scriptName;
			ScriptText = scriptText;

			SetupGlobalObjects();

		}
		
		private void SetupGlobalObjects()
		{
			Script = ScriptManager.Runtime.InitScript(ScriptName, typeof(JS.Global));
		}

		public void Execute()
		{
			if (ScriptText == null)
				return;

			Script.Compile(ScriptText);
			Script.Execute();
		}

		public string Eval(string text)
		{
			return Script.Eval(text);
		}

		public void Unload()
		{
			ScriptManager.Runtime.DestroyScript(Script);
		}

	}
}
