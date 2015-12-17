using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Xml.Serialization;
using DMXEngine;
using DMXCommunication;
using Gtk;
using DMXForGamers;

public partial class MainWindow: Gtk.Window
{
	private TextEventEngine _engine = null;
	private ITextMonitor _textMonitor = null;
	private Timer _dmxUpdateTimer = null;
	private const int MAX_LINE_COUNT = 200;
	private AppSettings _appSettings = null;
	private string _appSettingsFilePath = null;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		_dmxUpdateTimer = new Timer ();
		_dmxUpdateTimer.Interval = 50;
		_dmxUpdateTimer.Elapsed += DMXUpdateTimer_Elapsed;

		_stopButton.Sensitive = false;

		_appSettingsFilePath = Environment.CurrentDirectory + "\\appsetting.xml";
		_configFilePath.Text = _appSettingsFilePath;

		if (File.Exists (_appSettingsFilePath) == true) {
			try {
				_appSettings = AppSettingsFile.LoadFile (_appSettingsFilePath);
			} catch (Exception ex) {
				MessageDialog md = new MessageDialog (this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
					                   MessageType.Error, ButtonsType.Ok,
					                   "Unable to load to settings.\n\nDetails: " + ex.Message);
				md.Run ();
				md.Destroy ();

				_appSettings = new AppSettings ();
			}
		} else {
			_appSettings = new AppSettings ();
		}

		// PEV - 12/7/2015 - ComboBox model help
		// http://mono.1490590.n4.nabble.com/Combobox-with-values-from-databases-td4663160.html

		var dmxPortAdapters = DMXPortAdapterHelpers.GetPortAdapters ();
		var listStore = new ListStore (typeof(DMXCommunication.DMXPortAdapter));
		foreach (var element in dmxPortAdapters) {
			listStore.AppendValues (element);
		}
		_protocolComboBox.Clear ();
		_protocolComboBox.Model = listStore;
		var cellRenderText = new CellRendererText ();
		cellRenderText.Alignment = Pango.Alignment.Left;
		_protocolComboBox.PackStart (cellRenderText, false);
		_protocolComboBox.SetCellDataFunc (cellRenderText, new Gtk.CellLayoutDataFunc (ProtocolComboBox_GetDescription));

		TreeIter iter;
		_protocolComboBox.Model.GetIterFirst (out iter);
		do {
			var dmxPortAdapter = _protocolComboBox.Model.GetValue (iter, 0) as DMXPortAdapter;
			if(dmxPortAdapter.Id == _appSettings.PortAdapterGuid)
			{
				_protocolComboBox.SetActiveIter(iter);
				break;
			}
		} while(_protocolComboBox.Model.IterNext (ref iter) == true);

		_exeFile.Text = _appSettings.EXEFilePath;
		_exeArgs.Text = _appSettings.EXEArguments;
		_fileFile.Text = _appSettings.MonitorFilePath;
		_dmxFile.Text = _appSettings.DMXFilePath;
		_eventsFile.Text = _appSettings.EventDefinitionsFilePath;	

		Validate ();	
	}

	protected void ProtocolComboBox_GetDescription (CellLayout cell_layout, CellRenderer cell, TreeModel model, TreeIter iter)
	{
		var item = (DMXPortAdapter)model.GetValue (iter, 0);
		if (item != null) {
			(cell as Gtk.CellRendererText).Text = item.Description;
		}
	}

	void DMXUpdateTimer_Elapsed (object sender, ElapsedEventArgs e)
	{
		_dmxUpdateTimer.Enabled = false;
		try {
			if (_engine != null) {
				_engine.Execute (DateTime.Now);
			}
		} finally {
			_dmxUpdateTimer.Enabled = true;
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		OnStopButtonClicked (this, null);

		if (_dmxUpdateTimer != null) {
			_dmxUpdateTimer.Dispose ();
			_dmxUpdateTimer = null;
		}

		TreeIter iter;
		_protocolComboBox.GetActiveIter (out iter);
		var dmxPortAdapter = _protocolComboBox.Model.GetValue (iter, 0) as DMXPortAdapter;
		if (dmxPortAdapter != null) {
			_appSettings.PortAdapterGuid = dmxPortAdapter.Id;
		}

		_appSettings.EXEFilePath = _exeFile.Text;
		_appSettings.EXEArguments = _exeArgs.Text;
		_appSettings.MonitorFilePath = _fileFile.Text;
		_appSettings.EventDefinitionsFilePath = _eventsFile.Text;
		_appSettings.DMXFilePath = _dmxFile.Text;

		ResponseType response = ResponseType.Yes;
		while (response == ResponseType.Yes) {
			try {
				AppSettingsFile.SaveFile (_appSettings, _appSettingsFilePath);
				break;
			} catch (Exception ex) {
				MessageDialog md = new MessageDialog (this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
					                   MessageType.Error, ButtonsType.YesNo,
					                   "Unable to save settings.  Do you want to try again?  Otherwise current settings will be lost.\n\nDetails: " + ex.Message);
				response = (ResponseType)md.Run ();
				md.Destroy ();
			}
		}

		Application.Quit ();
		a.RetVal = true;
	}

	private void ProcessLine (string line)
	{
		if (line != null) {
			Gtk.Application.Invoke (delegate {
				AddLineToOutputTextView (line);
			});

			ProcessDMXEvent (line);
		}
	}

	private void AddLineToOutputTextView (string newLine)
	{
		int count = _outputTextView.Buffer.LineCount;
		if (count > MAX_LINE_COUNT) {
			TextIter lineStartIter = _outputTextView.Buffer.GetIterAtLine (0);
			TextIter lineEndIter = _outputTextView.Buffer.GetIterAtLine (_outputTextView.Buffer.LineCount - MAX_LINE_COUNT);
			_outputTextView.Buffer.Delete (ref lineStartIter, ref lineEndIter);
		}

		var endIter = _outputTextView.Buffer.EndIter;
		_outputTextView.Buffer.Insert (ref endIter, newLine + "\n");
		_outputTextView.ScrollToIter (_outputTextView.Buffer.EndIter, 0, true, 0, 0);
	}

	private void ProcessDMXEvent (string line)
	{
		if (_engine != null) {
			_engine.ProcessText (line);
		}
	}

	protected void OnStartButtonClicked (object sender, EventArgs e)
	{
		_startButton.Sensitive = false;
		_stopButton.Sensitive = true;

		try {
			CheckAndKillExistingProcesses ();

			TreeIter iter;
			_protocolComboBox.GetActiveIter (out iter);
			var dmxPortAdapter = _protocolComboBox.Model.GetValue (iter, 0) as DMXPortAdapter;
			IDMXCommunication dmxComm = null;
			if (dmxPortAdapter != null) {
				dmxComm = (IDMXCommunication)Activator.CreateInstance (dmxPortAdapter.Type);
			}

			if (dmxComm == null) {
				MessageDialog md = new MessageDialog (this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
					                   MessageType.Error, ButtonsType.Ok,
					                   "DMX Adapter/Port is Not Selected");
				md.Run ();
				md.Destroy ();
			} else {
				DMXStateMachine dmx = new DMXStateMachine (DMXEventsFile.LoadFile (_dmxFile.Text), dmxComm);

				_engine = new TextEventEngine (dmx, EventDefinitionsFile.LoadFile (_eventsFile.Text));

				_dmxUpdateTimer.Enabled = true;

				if (_monitorNotebook.CurrentPage == 0) {
					_textMonitor = new NewProcessTextMonitor (_exeFile.Text, _exeArgs.Text, ProcessLine);
				} else if (_monitorNotebook.CurrentPage == 1) {
					_textMonitor = new FileTextMonitor (_fileFile.Text, ProcessLine);
				} else {
					throw new Exception ("Invalid selection");
				}
				_textMonitor.Start ();
			}
		} catch (Exception ex) {
			MessageDialog md = new MessageDialog (this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
				                   MessageType.Error, ButtonsType.Ok,
				                   "Unhandled exception occured.\n\nDetails: " + ex.Message);
			md.Run ();
			md.Destroy ();

			OnStopButtonClicked (this, null);
		}
	}

	private void CheckAndKillExistingProcesses ()
	{
		var existingServers = Process.GetProcessesByName (System.IO.Path.GetFileNameWithoutExtension (_exeFile.Text));
		if (existingServers.Length > 0) {
			var dialog = new MessageDialog (this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
				             MessageType.Warning, ButtonsType.YesNo,
				             "EXE instance(s) are already running.  Do you wish to terminate them?");
			int response = dialog.Run ();
			dialog.Destroy ();

			if (response == (int)ResponseType.Yes) {
				foreach (var process in existingServers) {
					process.Kill ();
					process.WaitForExit ();
				}
			}
		}		
	}

	protected void OnStopButtonClicked (object sender, EventArgs e)
	{
		_startButton.Sensitive = true;
		_stopButton.Sensitive = false;

		if (_dmxUpdateTimer != null) {
			_dmxUpdateTimer.Enabled = false;
		}

		if (_engine != null) {
			_engine.Dispose ();
			_engine = null;
		}

		if (_textMonitor != null) {
			_textMonitor.Dispose ();
			_textMonitor = null;
		}
	}

	protected void Validate ()
	{
		bool problemsFound = false;

		if (_monitorNotebook.CurrentPage == 0) {
			if (File.Exists (_exeFile.Text) == true) {
				_exeFileWarning.Visible = false;
			} else {
				_exeFileWarning.Visible = true;
				problemsFound = true;
			}
		} else if (_monitorNotebook.CurrentPage == 1) {
			if (File.Exists (_fileFile.Text) == true) {
				_fileFileWarning.Visible = false;
			} else {
				_fileFileWarning.Visible = true;
				problemsFound = true;
			}
		}
		if (File.Exists (_eventsFile.Text) == true) {
			_eventsFileWarning.Visible = false;
		} else {
			_eventsFileWarning.Visible = true;
			problemsFound = true;
		}
		if (File.Exists (_dmxFile.Text) == true) {
			_dmxFileWarning.Visible = false;
		} else {
			_dmxFileWarning.Visible = true;
			problemsFound = true;
		}

		_startButton.Sensitive = !problemsFound;
	}

	protected void OnExeFileFocusOutEvent (object o, FocusOutEventArgs args)
	{
		Validate ();
	}

	protected void OnFileFileFocusOutEvent (object o, FocusOutEventArgs args)
	{
		Validate ();
	}

	protected void OnExeArgsFocusOutEvent (object o, FocusOutEventArgs args)
	{
		Validate ();
	}

	protected void OnEventsFileFocusOutEvent (object o, FocusOutEventArgs args)
	{
		Validate ();
	}

	protected void OnDmxFileFocusOutEvent (object o, FocusOutEventArgs args)
	{
		Validate ();
	}

	protected void OnExeFileButtonClicked (object sender, EventArgs e)
	{
		var dlg = new FileChooserDialog ("Select the Executable to Run", this,
			          FileChooserAction.Open, new object[] { "Open", ResponseType.Ok });
		dlg.SetUri (_exeFile.Text);
		if (dlg.Run () == (int)ResponseType.Ok) {
			_exeFile.Text = dlg.Filename;
			Validate ();
		}
		dlg.Destroy ();
	}

	protected void OnFileFileButtonClicked (object sender, EventArgs e)
	{
		var dlg = new FileChooserDialog ("Select the File to Monitor", this,
			          FileChooserAction.Open, new object[] { "Open", ResponseType.Ok });
		dlg.SetUri (_fileFile.Text);
		if (dlg.Run () == (int)ResponseType.Ok) {
			_fileFile.Text = dlg.Filename;
			Validate ();
		}
		dlg.Destroy ();
	}

	protected void OnEventsFileButtonClicked (object sender, EventArgs e)
	{
		var dlg = new FileChooserDialog ("Select Events Definition to Use", this,
			          FileChooserAction.Open, new object[] { "Open", ResponseType.Ok });
		dlg.SetUri (_eventsFile.Text);
		if (dlg.Run () == (int)ResponseType.Ok) {
			_eventsFile.Text = dlg.Filename;
			Validate ();
		}
		dlg.Destroy ();
	}

	protected void OnDmxFileButtonClicked (object sender, EventArgs e)
	{
		var dlg = new FileChooserDialog ("Select DMX Configuration to Use", this,
			          FileChooserAction.Open, new object[] { "Open", ResponseType.Ok });
		dlg.SetUri (_dmxFile.Text);
		if (dlg.Run () == (int)ResponseType.Ok) {
			_dmxFile.Text = dlg.Filename;
			Validate ();
		}
		dlg.Destroy ();
	}

	protected void OnMonitorNotebookSwitchPage (object o, SwitchPageArgs args)
	{
		Validate ();
	}

	protected void OnTestButtonClicked (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}
}
