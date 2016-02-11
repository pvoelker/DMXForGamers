
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.Table table1;
	
	private global::Gtk.Entry _dmxFile;
	
	private global::Gtk.Button _dmxFileButton;
	
	private global::Gtk.Label _dmxFileInstructions;
	
	private global::Gtk.Image _dmxFileWarning;
	
	private global::Gtk.Entry _eventsFile;
	
	private global::Gtk.Button _eventsFileButton;
	
	private global::Gtk.Label _eventsFileInstructions;
	
	private global::Gtk.Image _eventsFileWarning;
	
	private global::Gtk.Notebook _monitorNotebook;
	
	private global::Gtk.Table table2;
	
	private global::Gtk.Entry _exeArgs;
	
	private global::Gtk.Entry _exeFile;
	
	private global::Gtk.Button _exeFileButton;
	
	private global::Gtk.Image _exeFileWarning;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.Label label4;
	
	private global::Gtk.Label _NewProcessMonitorSettingsLabel;
	
	private global::Gtk.Table table3;
	
	private global::Gtk.Entry _fileFile;
	
	private global::Gtk.Button _fileFileButton;
	
	private global::Gtk.Image _fileFileWarning;
	
	private global::Gtk.Label label6;
	
	private global::Gtk.Label _FileMonitorSettingsLabel;
	
	private global::Gtk.Label _eventsFileInstructions1;
	
	private global::Gtk.Label _NoMonitorSettingsLabel;
	
	private global::Gtk.ComboBox _protocolComboBox;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.Label label5;
	
	private global::Gtk.Notebook notebook1;
	
	private global::Gtk.VBox vbox2;
	
	private global::Gtk.HBox hbox1;
	
	private global::Gtk.Button _startButton;
	
	private global::Gtk.Button _stopButton;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	
	private global::Gtk.TextView _outputTextView;
	
	private global::Gtk.Label label8;
	
	private global::DMXForGamers.ManualEventsConsole _manualEvents;
	
	private global::Gtk.Label label9;
	
	private global::Gtk.Label _configFilePath;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("DMX for Gamers");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.DefaultWidth = 640;
		this.DefaultHeight = 480;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.table1 = new global::Gtk.Table (((uint)(6)), ((uint)(4)), false);
		this.table1.Name = "table1";
		this.table1.RowSpacing = ((uint)(6));
		this.table1.ColumnSpacing = ((uint)(6));
		this.table1.BorderWidth = ((uint)(3));
		// Container child table1.Gtk.Table+TableChild
		this._dmxFile = new global::Gtk.Entry ();
		this._dmxFile.CanFocus = true;
		this._dmxFile.Name = "_dmxFile";
		this._dmxFile.IsEditable = true;
		this._dmxFile.InvisibleChar = '●';
		this.table1.Add (this._dmxFile);
		global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1 [this._dmxFile]));
		w1.TopAttach = ((uint)(5));
		w1.BottomAttach = ((uint)(6));
		w1.LeftAttach = ((uint)(1));
		w1.RightAttach = ((uint)(2));
		w1.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._dmxFileButton = new global::Gtk.Button ();
		this._dmxFileButton.CanFocus = true;
		this._dmxFileButton.Name = "_dmxFileButton";
		this._dmxFileButton.UseUnderline = true;
		this._dmxFileButton.Label = global::Mono.Unix.Catalog.GetString ("...");
		this.table1.Add (this._dmxFileButton);
		global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this._dmxFileButton]));
		w2.TopAttach = ((uint)(5));
		w2.BottomAttach = ((uint)(6));
		w2.LeftAttach = ((uint)(2));
		w2.RightAttach = ((uint)(3));
		w2.XOptions = ((global::Gtk.AttachOptions)(4));
		w2.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._dmxFileInstructions = new global::Gtk.Label ();
		this._dmxFileInstructions.Name = "_dmxFileInstructions";
		this._dmxFileInstructions.LabelProp = global::Mono.Unix.Catalog.GetString ("<span foreground=\"blue\" size=\"large\">The \'DMX File\' defines DMX sequences for par" +
		"ticular events...</span>");
		this._dmxFileInstructions.UseMarkup = true;
		this.table1.Add (this._dmxFileInstructions);
		global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this._dmxFileInstructions]));
		w3.TopAttach = ((uint)(4));
		w3.BottomAttach = ((uint)(5));
		w3.RightAttach = ((uint)(2));
		w3.XOptions = ((global::Gtk.AttachOptions)(4));
		w3.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._dmxFileWarning = new global::Gtk.Image ();
		this._dmxFileWarning.Name = "_dmxFileWarning";
		this._dmxFileWarning.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("DMXForGamers.Images.Warning24x24.png");
		this.table1.Add (this._dmxFileWarning);
		global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1 [this._dmxFileWarning]));
		w4.TopAttach = ((uint)(5));
		w4.BottomAttach = ((uint)(6));
		w4.LeftAttach = ((uint)(3));
		w4.RightAttach = ((uint)(4));
		w4.XOptions = ((global::Gtk.AttachOptions)(4));
		w4.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._eventsFile = new global::Gtk.Entry ();
		this._eventsFile.CanFocus = true;
		this._eventsFile.Name = "_eventsFile";
		this._eventsFile.IsEditable = true;
		this._eventsFile.InvisibleChar = '●';
		this.table1.Add (this._eventsFile);
		global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this._eventsFile]));
		w5.TopAttach = ((uint)(3));
		w5.BottomAttach = ((uint)(4));
		w5.LeftAttach = ((uint)(1));
		w5.RightAttach = ((uint)(2));
		w5.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._eventsFileButton = new global::Gtk.Button ();
		this._eventsFileButton.CanFocus = true;
		this._eventsFileButton.Name = "_eventsFileButton";
		this._eventsFileButton.UseUnderline = true;
		this._eventsFileButton.Label = global::Mono.Unix.Catalog.GetString ("...");
		this.table1.Add (this._eventsFileButton);
		global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this._eventsFileButton]));
		w6.TopAttach = ((uint)(3));
		w6.BottomAttach = ((uint)(4));
		w6.LeftAttach = ((uint)(2));
		w6.RightAttach = ((uint)(3));
		w6.XOptions = ((global::Gtk.AttachOptions)(4));
		w6.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._eventsFileInstructions = new global::Gtk.Label ();
		this._eventsFileInstructions.Name = "_eventsFileInstructions";
		this._eventsFileInstructions.LabelProp = global::Mono.Unix.Catalog.GetString ("<span foreground=\"blue\" size=\"large\">The \'Events File\' is used to define events t" +
		"hat are triggered by a simple string match or regular expression match...</span>" +
		"");
		this._eventsFileInstructions.UseMarkup = true;
		this.table1.Add (this._eventsFileInstructions);
		global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this._eventsFileInstructions]));
		w7.TopAttach = ((uint)(2));
		w7.BottomAttach = ((uint)(3));
		w7.RightAttach = ((uint)(2));
		w7.XOptions = ((global::Gtk.AttachOptions)(4));
		w7.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._eventsFileWarning = new global::Gtk.Image ();
		this._eventsFileWarning.Name = "_eventsFileWarning";
		this._eventsFileWarning.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("DMXForGamers.Images.Warning24x24.png");
		this.table1.Add (this._eventsFileWarning);
		global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this._eventsFileWarning]));
		w8.TopAttach = ((uint)(3));
		w8.BottomAttach = ((uint)(4));
		w8.LeftAttach = ((uint)(3));
		w8.RightAttach = ((uint)(4));
		w8.XOptions = ((global::Gtk.AttachOptions)(4));
		w8.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._monitorNotebook = new global::Gtk.Notebook ();
		this._monitorNotebook.CanFocus = true;
		this._monitorNotebook.Name = "_monitorNotebook";
		this._monitorNotebook.CurrentPage = 2;
		// Container child _monitorNotebook.Gtk.Notebook+NotebookChild
		this.table2 = new global::Gtk.Table (((uint)(2)), ((uint)(4)), false);
		this.table2.Name = "table2";
		this.table2.RowSpacing = ((uint)(6));
		this.table2.ColumnSpacing = ((uint)(6));
		this.table2.BorderWidth = ((uint)(3));
		// Container child table2.Gtk.Table+TableChild
		this._exeArgs = new global::Gtk.Entry ();
		this._exeArgs.CanFocus = true;
		this._exeArgs.Name = "_exeArgs";
		this._exeArgs.IsEditable = true;
		this._exeArgs.InvisibleChar = '●';
		this.table2.Add (this._exeArgs);
		global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table2 [this._exeArgs]));
		w9.TopAttach = ((uint)(1));
		w9.BottomAttach = ((uint)(2));
		w9.LeftAttach = ((uint)(1));
		w9.RightAttach = ((uint)(2));
		w9.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table2.Gtk.Table+TableChild
		this._exeFile = new global::Gtk.Entry ();
		this._exeFile.CanFocus = true;
		this._exeFile.Name = "_exeFile";
		this._exeFile.IsEditable = true;
		this._exeFile.InvisibleChar = '●';
		this.table2.Add (this._exeFile);
		global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table2 [this._exeFile]));
		w10.LeftAttach = ((uint)(1));
		w10.RightAttach = ((uint)(2));
		w10.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table2.Gtk.Table+TableChild
		this._exeFileButton = new global::Gtk.Button ();
		this._exeFileButton.CanFocus = true;
		this._exeFileButton.Name = "_exeFileButton";
		this._exeFileButton.UseUnderline = true;
		this._exeFileButton.Label = global::Mono.Unix.Catalog.GetString ("...");
		this.table2.Add (this._exeFileButton);
		global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table2 [this._exeFileButton]));
		w11.LeftAttach = ((uint)(2));
		w11.RightAttach = ((uint)(3));
		w11.XOptions = ((global::Gtk.AttachOptions)(4));
		w11.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table2.Gtk.Table+TableChild
		this._exeFileWarning = new global::Gtk.Image ();
		this._exeFileWarning.Name = "_exeFileWarning";
		this._exeFileWarning.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("DMXForGamers.Images.Warning24x24.png");
		this.table2.Add (this._exeFileWarning);
		global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table2 [this._exeFileWarning]));
		w12.LeftAttach = ((uint)(3));
		w12.RightAttach = ((uint)(4));
		w12.XOptions = ((global::Gtk.AttachOptions)(4));
		w12.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table2.Gtk.Table+TableChild
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("EXE File");
		this.table2.Add (this.label3);
		global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table2 [this.label3]));
		w13.XOptions = ((global::Gtk.AttachOptions)(4));
		w13.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table2.Gtk.Table+TableChild
		this.label4 = new global::Gtk.Label ();
		this.label4.Name = "label4";
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("EXE Args");
		this.table2.Add (this.label4);
		global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table2 [this.label4]));
		w14.TopAttach = ((uint)(1));
		w14.BottomAttach = ((uint)(2));
		w14.XOptions = ((global::Gtk.AttachOptions)(4));
		w14.YOptions = ((global::Gtk.AttachOptions)(4));
		this._monitorNotebook.Add (this.table2);
		// Notebook tab
		this._NewProcessMonitorSettingsLabel = new global::Gtk.Label ();
		this._NewProcessMonitorSettingsLabel.Name = "_NewProcessMonitorSettingsLabel";
		this._NewProcessMonitorSettingsLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("New Process Monitor");
		this._monitorNotebook.SetTabLabel (this.table2, this._NewProcessMonitorSettingsLabel);
		this._NewProcessMonitorSettingsLabel.ShowAll ();
		// Container child _monitorNotebook.Gtk.Notebook+NotebookChild
		this.table3 = new global::Gtk.Table (((uint)(1)), ((uint)(4)), false);
		this.table3.Name = "table3";
		this.table3.RowSpacing = ((uint)(6));
		this.table3.ColumnSpacing = ((uint)(6));
		this.table3.BorderWidth = ((uint)(3));
		// Container child table3.Gtk.Table+TableChild
		this._fileFile = new global::Gtk.Entry ();
		this._fileFile.CanFocus = true;
		this._fileFile.Name = "_fileFile";
		this._fileFile.IsEditable = true;
		this._fileFile.InvisibleChar = '●';
		this.table3.Add (this._fileFile);
		global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table3 [this._fileFile]));
		w16.LeftAttach = ((uint)(1));
		w16.RightAttach = ((uint)(2));
		w16.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table3.Gtk.Table+TableChild
		this._fileFileButton = new global::Gtk.Button ();
		this._fileFileButton.CanFocus = true;
		this._fileFileButton.Name = "_fileFileButton";
		this._fileFileButton.UseUnderline = true;
		this._fileFileButton.Label = global::Mono.Unix.Catalog.GetString ("...");
		this.table3.Add (this._fileFileButton);
		global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table3 [this._fileFileButton]));
		w17.LeftAttach = ((uint)(2));
		w17.RightAttach = ((uint)(3));
		w17.XOptions = ((global::Gtk.AttachOptions)(4));
		w17.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table3.Gtk.Table+TableChild
		this._fileFileWarning = new global::Gtk.Image ();
		this._fileFileWarning.Name = "_fileFileWarning";
		this._fileFileWarning.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("DMXForGamers.Images.Warning24x24.png");
		this.table3.Add (this._fileFileWarning);
		global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table3 [this._fileFileWarning]));
		w18.LeftAttach = ((uint)(3));
		w18.RightAttach = ((uint)(4));
		w18.XOptions = ((global::Gtk.AttachOptions)(4));
		w18.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table3.Gtk.Table+TableChild
		this.label6 = new global::Gtk.Label ();
		this.label6.Name = "label6";
		this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("File");
		this.table3.Add (this.label6);
		global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table3 [this.label6]));
		w19.XOptions = ((global::Gtk.AttachOptions)(4));
		w19.YOptions = ((global::Gtk.AttachOptions)(4));
		this._monitorNotebook.Add (this.table3);
		global::Gtk.Notebook.NotebookChild w20 = ((global::Gtk.Notebook.NotebookChild)(this._monitorNotebook [this.table3]));
		w20.Position = 1;
		// Notebook tab
		this._FileMonitorSettingsLabel = new global::Gtk.Label ();
		this._FileMonitorSettingsLabel.Name = "_FileMonitorSettingsLabel";
		this._FileMonitorSettingsLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("File Monitor");
		this._monitorNotebook.SetTabLabel (this.table3, this._FileMonitorSettingsLabel);
		this._FileMonitorSettingsLabel.ShowAll ();
		// Container child _monitorNotebook.Gtk.Notebook+NotebookChild
		this._eventsFileInstructions1 = new global::Gtk.Label ();
		this._eventsFileInstructions1.Name = "_eventsFileInstructions1";
		this._eventsFileInstructions1.LabelProp = global::Mono.Unix.Catalog.GetString ("<span foreground=\"blue\" size=\"large\">Do not run any text monitor.  Manual trigger" +
		"ing of events only...</span>");
		this._eventsFileInstructions1.UseMarkup = true;
		this._monitorNotebook.Add (this._eventsFileInstructions1);
		global::Gtk.Notebook.NotebookChild w21 = ((global::Gtk.Notebook.NotebookChild)(this._monitorNotebook [this._eventsFileInstructions1]));
		w21.Position = 2;
		// Notebook tab
		this._NoMonitorSettingsLabel = new global::Gtk.Label ();
		this._NoMonitorSettingsLabel.Name = "_NoMonitorSettingsLabel";
		this._NoMonitorSettingsLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("No Monitor");
		this._monitorNotebook.SetTabLabel (this._eventsFileInstructions1, this._NoMonitorSettingsLabel);
		this._NoMonitorSettingsLabel.ShowAll ();
		this.table1.Add (this._monitorNotebook);
		global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.table1 [this._monitorNotebook]));
		w22.TopAttach = ((uint)(1));
		w22.BottomAttach = ((uint)(2));
		w22.RightAttach = ((uint)(4));
		w22.XPadding = ((uint)(3));
		w22.YPadding = ((uint)(3));
		w22.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this._protocolComboBox = global::Gtk.ComboBox.NewText ();
		this._protocolComboBox.Name = "_protocolComboBox";
		this.table1.Add (this._protocolComboBox);
		global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.table1 [this._protocolComboBox]));
		w23.LeftAttach = ((uint)(1));
		w23.RightAttach = ((uint)(2));
		w23.XOptions = ((global::Gtk.AttachOptions)(4));
		w23.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Events File");
		this.table1.Add (this.label1);
		global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
		w24.TopAttach = ((uint)(3));
		w24.BottomAttach = ((uint)(4));
		w24.XOptions = ((global::Gtk.AttachOptions)(4));
		w24.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Protocol");
		this.label2.UseMarkup = true;
		this.table1.Add (this.label2);
		global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.table1 [this.label2]));
		w25.XOptions = ((global::Gtk.AttachOptions)(4));
		w25.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label5 = new global::Gtk.Label ();
		this.label5.Name = "label5";
		this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("DMX File");
		this.table1.Add (this.label5);
		global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.table1 [this.label5]));
		w26.TopAttach = ((uint)(5));
		w26.BottomAttach = ((uint)(6));
		w26.XOptions = ((global::Gtk.AttachOptions)(4));
		w26.YOptions = ((global::Gtk.AttachOptions)(4));
		this.vbox1.Add (this.table1);
		global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.table1]));
		w27.Position = 0;
		w27.Expand = false;
		w27.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.notebook1 = new global::Gtk.Notebook ();
		this.notebook1.CanFocus = true;
		this.notebook1.Name = "notebook1";
		this.notebook1.CurrentPage = 0;
		// Container child notebook1.Gtk.Notebook+NotebookChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Homogeneous = true;
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this._startButton = new global::Gtk.Button ();
		this._startButton.CanFocus = true;
		this._startButton.Name = "_startButton";
		this._startButton.UseUnderline = true;
		this._startButton.Label = global::Mono.Unix.Catalog.GetString ("Start");
		global::Gtk.Image w28 = new global::Gtk.Image ();
		w28.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("DMXForGamers.Images.Warning24x24.png");
		this._startButton.Image = w28;
		this.hbox1.Add (this._startButton);
		global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this._startButton]));
		w29.Position = 0;
		w29.Expand = false;
		w29.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this._stopButton = new global::Gtk.Button ();
		this._stopButton.CanFocus = true;
		this._stopButton.Name = "_stopButton";
		this._stopButton.UseUnderline = true;
		this._stopButton.Label = global::Mono.Unix.Catalog.GetString ("Stop");
		global::Gtk.Image w30 = new global::Gtk.Image ();
		w30.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("DMXForGamers.Images.Warning24x24.png");
		this._stopButton.Image = w30;
		this.hbox1.Add (this._stopButton);
		global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this._stopButton]));
		w31.Position = 1;
		w31.Expand = false;
		w31.Fill = false;
		this.vbox2.Add (this.hbox1);
		global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox1]));
		w32.Position = 0;
		w32.Expand = false;
		w32.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this._outputTextView = new global::Gtk.TextView ();
		this._outputTextView.CanFocus = true;
		this._outputTextView.Name = "_outputTextView";
		this._outputTextView.Editable = false;
		this.GtkScrolledWindow.Add (this._outputTextView);
		this.vbox2.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w34 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow]));
		w34.Position = 1;
		this.notebook1.Add (this.vbox2);
		// Notebook tab
		this.label8 = new global::Gtk.Label ();
		this.label8.Name = "label8";
		this.label8.LabelProp = global::Mono.Unix.Catalog.GetString ("Main Control");
		this.notebook1.SetTabLabel (this.vbox2, this.label8);
		this.label8.ShowAll ();
		// Container child notebook1.Gtk.Notebook+NotebookChild
		this._manualEvents = new global::DMXForGamers.ManualEventsConsole ();
		this._manualEvents.Events = ((global::Gdk.EventMask)(256));
		this._manualEvents.Name = "_manualEvents";
		this.notebook1.Add (this._manualEvents);
		global::Gtk.Notebook.NotebookChild w36 = ((global::Gtk.Notebook.NotebookChild)(this.notebook1 [this._manualEvents]));
		w36.Position = 1;
		// Notebook tab
		this.label9 = new global::Gtk.Label ();
		this.label9.Name = "label9";
		this.label9.LabelProp = global::Mono.Unix.Catalog.GetString ("Manual Triggering");
		this.notebook1.SetTabLabel (this._manualEvents, this.label9);
		this.label9.ShowAll ();
		this.vbox1.Add (this.notebook1);
		global::Gtk.Box.BoxChild w37 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.notebook1]));
		w37.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this._configFilePath = new global::Gtk.Label ();
		this._configFilePath.Name = "_configFilePath";
		this._configFilePath.Justify = ((global::Gtk.Justification)(1));
		this.vbox1.Add (this._configFilePath);
		global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this._configFilePath]));
		w38.Position = 2;
		w38.Expand = false;
		w38.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this._monitorNotebook.SwitchPage += new global::Gtk.SwitchPageHandler (this.OnMonitorNotebookSwitchPage);
		this._exeFileButton.Clicked += new global::System.EventHandler (this.OnExeFileButtonClicked);
		this._exeFile.FocusOutEvent += new global::Gtk.FocusOutEventHandler (this.OnExeFileFocusOutEvent);
		this._fileFileButton.Clicked += new global::System.EventHandler (this.OnFileFileButtonClicked);
		this._fileFile.FocusOutEvent += new global::Gtk.FocusOutEventHandler (this.OnFileFileFocusOutEvent);
		this._eventsFileButton.Clicked += new global::System.EventHandler (this.OnEventsFileButtonClicked);
		this._dmxFileButton.Clicked += new global::System.EventHandler (this.OnDmxFileButtonClicked);
		this._startButton.Clicked += new global::System.EventHandler (this.OnStartButtonClicked);
	}
}
