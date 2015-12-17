using System;
using Gtk;

namespace DMXForGamers
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			Settings.Default.SetLongProperty ("gtk-button-images", 1, "");
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
