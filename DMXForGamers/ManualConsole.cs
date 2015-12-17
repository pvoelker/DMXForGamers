using System;
using DMXCommunication;
using Gtk;

namespace DMXForGamers
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ManualConsole : Gtk.Bin
	{
		private IDMXCommunication _dmx = new OpenDMX();

		public ManualConsole ()
		{
			this.Build ();

			BuildConsole ();

			_dmx.Start ();
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			_dmx.Stop ();
		}

		private void BuildConsole(){
			_consoleTable.NRows = 5;
			_consoleTable.NColumns = 256;

			for (uint i = 0; i < 256; i++) {
				var newLabel = new Gtk.Label ();
				newLabel.Markup = "<b>" + (i + 256 + 1).ToString() + "</b>";
				_consoleTable.Attach (newLabel, i, i + 1, 0, 1);
				var newVScale = new Gtk.VScale (0, 255, 1);
				newVScale.ValuePos = Gtk.PositionType.Bottom;
				newVScale.Inverted = true;
				newVScale.Data["Channel"] = i + 256 + 1;
				newVScale.ChangeValue += NewVScale_ChangeValue;
				newVScale.SetSizeRequest(-1, 100);
				_consoleTable.Attach (newVScale, i, i + 1, 1, 2);
			}
			_consoleTable.Attach (new Gtk.HSeparator (), 0, 256, 2, 3);
			for (uint i = 0; i < 256; i++) {
				var newLabel = new Gtk.Label ();
				newLabel.Markup = "<b>" + (i + 1).ToString() + "</b>";
				_consoleTable.Attach (newLabel, i, i + 1, 3, 4);
				var newVScale = new Gtk.VScale (0, 255, 1);
				newVScale.ValuePos = Gtk.PositionType.Bottom;
				newVScale.Inverted = true;
				newVScale.Data["Channel"] = i + 1;
				newVScale.ChangeValue += NewVScale_ChangeValue;
				newVScale.SetSizeRequest(-1, 100);
				_consoleTable.Attach (newVScale, i, i + 1, 4, 5);
			}
		}

		void NewVScale_ChangeValue (object o, Gtk.ChangeValueArgs args)
		{
			var vScale = o as Gtk.VScale;
			if (vScale != null) {
				var channel = (uint)vScale.Data["Channel"];
				_dmx.SetChannelValue ((ushort)channel, (byte)vScale.Value);
			}
		}
	}
}

