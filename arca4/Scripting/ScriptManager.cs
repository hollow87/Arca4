using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using smnetjs;

namespace arca4.Scripting
{
	sealed class ScriptManager
	{
		public sealed class ScriptManagerIndexer : IEnumerable<ScriptObj>
		{

			
			private List<ScriptObj> scripts = null;
			


			public ScriptManagerIndexer()
			{
				scripts = new List<ScriptObj>();
				
			}
			

			public ScriptObj this[int index]
			{
				get
				{
					if (scripts.Count < index)
						throw new ArgumentOutOfRangeException("index");

					return scripts[index];
				}
			}

			public ScriptObj this[string scriptName]
			{
				get
				{

					// TODO: Add error detection?

					ScriptObj script = scripts.Where(c => scriptName.Equals(c.ScriptName)).First();

					return script;

				}
			}

			public int Count { get { return scripts.Count; } }

			public void Add(string scriptName, string scriptText = null)
			{

				if (Find(scriptName) != null)
					return;

				ScriptObj script = new ScriptObj(scriptName, scriptText);
				scripts.Add(script);
				script.Execute();
			}

			public void Remove(string scriptName)
			{
				if (scripts.Where(c => scriptName.Equals(c.ScriptName)).Count() == 0)
					return;

				this[scriptName].Unload();
				scripts.Remove(this[scriptName]);
			}

			public ScriptObj Find(string scriptName)
			{
				return scripts.Find(c => c.ScriptName.Equals(scriptName, StringComparison.InvariantCultureIgnoreCase));
			}


			public IEnumerator<ScriptObj> GetEnumerator()
			{
				return scripts.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return scripts.GetEnumerator();
			}
		}

		
		private static SMRuntime runtime = null;

		private static ScriptManagerIndexer staticIndexer;

		static ScriptManager()
		{
			runtime = new SMRuntime(RuntimeOptions.DisableAutoEmbedding | RuntimeOptions.ForceGCOnContextDestroy);

			
			bool ret = runtime.Embed(typeof(EventType));
			ret &= runtime.Embed(typeof(RejectionType));
			ret &= runtime.Embed(typeof(JS.UserObj));
			ret &= runtime.Embed(typeof(JS.Events));
			ret &= runtime.Embed(typeof(JS.Script));


			runtime.OnScriptError += new ScriptErrorHandler(runtime_OnScriptError);

			
		}

		static void runtime_OnScriptError(SMScript script, SMErrorReport report)
		{
			DebugLog.WriteLine(
				string.Format("error: {0} in {1} on line {2}", report.Message, script.Name, report.LineNumber));

		}


		public static ScriptManagerIndexer Items
		{
			get { return staticIndexer ?? (staticIndexer = new ScriptManagerIndexer()); }
		}

		internal static SMRuntime Runtime { get { return runtime; } }



	}
}
