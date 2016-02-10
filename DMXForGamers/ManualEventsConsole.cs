using System;
using System.Collections.Generic;
using System.Linq;
using DMXEngine;
using Gtk;

namespace DMXForGamers
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ManualEventsConsole : Gtk.Bin
	{
		private TextEventEngine _engine;
		private List<Gtk.ToggleButton> _sceneButtons = new List<Gtk.ToggleButton>();
		private List<Gtk.Button> _eventButtons = new List<Gtk.Button>();

		public ManualEventsConsole ()
		{
			this.Build ();
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			UnBuildConsole ();
		}

		public void ConfigureUI(TextEventEngine engine)
		{
			_engine = engine;

			BuildConsole ();
		}

		private void UnBuildConsole()
		{
			foreach(var button in _sceneButtons)
			{
				_sceneConsoleTable.Remove (button);
				button.Destroy ();
				button.Dispose ();
			}
			_sceneButtons.Clear ();			
			foreach(var button in _eventButtons)
			{
				_eventConsoleTable.Remove (button);
				button.Destroy ();
				button.Dispose ();
			}
			_eventButtons.Clear ();
		}

		private void BuildConsole()
		{
			UnBuildConsole ();

			_sceneConsoleTable.NColumns =  2;
			_sceneConsoleTable.NRows = (uint)(Math.Ceiling((_engine.EventDefinitions.Events.Where(x => x.Continuous == true).Count() + 1.0) / 2.0));

			_eventConsoleTable.NColumns =  2;
			_eventConsoleTable.NRows = (uint)(Math.Ceiling(_engine.EventDefinitions.Events.Where(x => x.Continuous == false).Count() / 2.0));

			uint currentRow = 0;
			uint currentColumn = 0;
			var sceneDefs = _engine.EventDefinitions.Events.Where(x => x.Continuous == true).ToList();
			sceneDefs.Insert (0, new EventDefinition ("Reset Scene", "_Reset_Scene_", ""));
			foreach(var item in sceneDefs)
			{
				var newToggle = new Gtk.ToggleButton ();
				newToggle.Label = item.EventID + " - " + item.Description;
				newToggle.Data["Event"] = item;
				newToggle.Toggled += OnButtonToggle;
				_sceneConsoleTable.Attach (newToggle, currentColumn, currentColumn + 1, currentRow, currentRow + 1);
				newToggle.Show ();

				_sceneButtons.Add (newToggle);

				currentColumn++;
				if (currentColumn % 2 == 0) {
					currentColumn = 0;
					currentRow++;
				}
			}		

			currentRow = 0;
			currentColumn = 0;
			foreach(var item in _engine.EventDefinitions.Events.Where(x => x.Continuous == false))
			{
				var newButton = new Gtk.Button ();
				newButton.Label = item.EventID + " - " + item.Description;
				newButton.Data["Event"] = item;
				newButton.Clicked += OnButtonClicked;
				_eventConsoleTable.Attach (newButton, currentColumn, currentColumn + 1, currentRow, currentRow + 1);
				newButton.Show ();

				_eventButtons.Add (newButton);

				currentColumn++;
				if (currentColumn % 2 == 0) {
					currentColumn = 0;
					currentRow++;
				}
			}				
		}

		protected void OnButtonToggle (object sender, EventArgs e)
		{
			var button = sender as Gtk.ToggleButton;
			var item = button.Data["Event"] as EventDefinition;

			foreach(var activeButton in _sceneButtons.Where(x => (x != button) && (x.Active == true)))
			{
				activeButton.Toggled -= OnButtonToggle;

				activeButton.Active = false;
				var activeItem = activeButton.Data ["Event"] as EventDefinition;
				if (activeItem != null) {
					_engine.ManualRemoveEvent (activeItem.EventID);
				}

				activeButton.Toggled += OnButtonToggle;
			}

			if (item != null) {
				if (button.Active == true) {
					_engine.ManualAddEvent (item.EventID, true);
				} else {
					_engine.ManualRemoveEvent (item.EventID);
				}
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

