using System;
using System.Collections.Generic;
using DMXEngine;

namespace DMXForGamers
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ManualEventsConsole : Gtk.Bin
	{
		private TextEventEngine _engine;
		private List<Gtk.Widget> _buttons = new List<Gtk.Widget>();

		public ManualEventsConsole ()
		{
			this.Build ();
		}

		public void ConfigureUI(TextEventEngine engine)
		{
			_engine = engine;

			BuildConsole ();
		}

		private void BuildConsole()
		{
			// DISPOSE???????
			foreach(var button in _buttons)
			{
				_consoleTable.Remove (button);
				button.Destroy ();
				button.Dispose ();
			}
			_buttons.Clear ();

			_consoleTable.NColumns =  2;
			_consoleTable.NRows = (uint)(Math.Ceiling(_engine.EventDefinitions.Events.Count / 2.0));

			uint currentRow = 0;
			uint currentColumn = 0;
//			Gtk.RadioToolButton toggleButtonGroup = null;
			foreach(var item in _engine.EventDefinitions.Events)
			{
				if (item.Continuous == true) {
					var newToggle = new Gtk.ToggleButton ();
//					var newToggle = null as Gtk.RadioToolButton;
//					if (toggleButtonGroup == null) {
//						newToggle = new Gtk.RadioToolButton ((GLib.SList)null);
//						toggleButtonGroup = newToggle;
//					} else {
//						newToggle = new Gtk.RadioToolButton ((GLib.SList)null/*toggleButtonGroup*/);
//					}
					newToggle.Label = item.EventID + " - " + item.Description;
					newToggle.Data["Event"] = item;
					newToggle.Toggled += OnButtonToggle;
					_consoleTable.Attach (newToggle, currentColumn, currentColumn + 1, currentRow, currentRow + 1);
					newToggle.Show ();

					_buttons.Add (newToggle);
				} else {
					var newButton = new Gtk.Button ();
					newButton.Label = item.EventID + " - " + item.Description;
					newButton.Data["Event"] = item;
					newButton.Clicked += OnButtonClicked;
					_consoleTable.Attach (newButton, currentColumn, currentColumn + 1, currentRow, currentRow + 1);
					newButton.Show ();

					_buttons.Add (newButton);
				}

				currentColumn++;
				if (currentColumn % 2 == 0) {
					currentColumn = 0;
					currentRow++;
				}
			}		
		}

		protected void OnButtonToggle (object sender, EventArgs e)
		{
			//var button = sender as Gtk.ToggleToolButton;
			var button = sender as Gtk.ToggleButton;
			var item = button.Data["Event"] as EventDefinition;

			if (button.Active == true) {
				_engine.ManualAddEvent (item.EventID, true);
			} else {
				_engine.ManualRemoveEvent (item.EventID);
			}
		}

		protected void OnButtonClicked (object sender, EventArgs e)
		{
			var button = sender as Gtk.Button;
			var item = button.Data["Event"] as EventDefinition;

			_engine.ManualAddEvent (item.EventID);
		}
	}
}

