using System;
using System.Text.RegularExpressions;

namespace DMXEngine
{
	public class TextEventEngine
	{
		private DMXStateMachine _dmx = null;
		private EventDefinitions _eventDefs = null;

		public TextEventEngine (DMXStateMachine dmx, EventDefinitions eventDefs)
		{
			_dmx = dmx;
			_eventDefs = eventDefs;
		}

		public EventDefinitions EventDefinitions { get { return _eventDefs; } }

		public void ProcessText (string line)
		{
			foreach (var eventDef in _eventDefs.Events) {
				if (eventDef.UseRegEx == true) {
					if (Regex.IsMatch (line, eventDef.Pattern,
						        RegexOptions.IgnoreCase | RegexOptions.Singleline) == true) {
						_dmx.AddEvent (eventDef.EventID);
						break;
					}
				} else {
					if (line.ToLower ().Contains (eventDef.Pattern.ToLower ()) == true) {
						_dmx.AddEvent (eventDef.EventID);
						break;
					}
				}
			}
		}

		public void ManualAddEvent(string eventName, bool continuous = false)
		{
			_dmx.AddEvent (eventName, continuous);
		}

		public void ManualRemoveEvent(string eventName)
		{
			_dmx.RemoveEvent (eventName);
		}

		public void Execute (DateTime dt)
		{
			_dmx.Execute (dt);
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		~TextEventEngine ()
		{
			Dispose (false);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (disposing) {
				// Free managed resources
				if (_dmx != null) {
					_dmx.Dispose ();
					_dmx = null;
				}
			}

			// Free native resources if there are any
		}

		#endregion
	}
}

