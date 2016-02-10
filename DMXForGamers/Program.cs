using System;
using Gtk;

namespace DMXForGamers
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			//AppDomain.CurrentDomain.UnhandledException += AppDomain_CurrentDomain_UnhandledException;
			Application.Run ();
		}

//		static void AppDomain_CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
//		{
//			MessageDialog dlg = new MessageDialog(null, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Error: " + e.ExceptionObject.ToString());
//		}
	}
}
