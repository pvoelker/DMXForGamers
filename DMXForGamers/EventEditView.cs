using System;
using DMXEngine;
using System.Collections.Generic;

namespace DMXForGamers
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class EventEditView : Gtk.Bin
	{
		private const int EVENT_COL_COUNT = 5;
		private const int EVENT_ID_COL = 0;
		private const int EVENT_DESCRIPTION_COL = 1;
		private const int EVENT_USE_REG_EX_COL = 2;
		private const int EVENT_PATTERN_COL = 3;
		private const int EVENT_DELETE_COL = 4;

		private Gtk.ListStore _listModel;

		public EventEditView ()
		{
			this.Build ();

			InitTable ();
		}

		private void InitTable ()
		{
			var IDCellRenderer = new Gtk.CellRendererText ();
			IDCellRenderer.Background = "light gray";
			IDCellRenderer.Editable = true;
			IDCellRenderer.Edited += IDCellRenderer_Edited;
			_eventsList.AppendColumn("ID", IDCellRenderer, "text", EVENT_ID_COL);
			var DescriptionCellRenderer = new Gtk.CellRendererText ();
			DescriptionCellRenderer.Editable = true;
			DescriptionCellRenderer.Edited += DescriptionCellRenderer_Edited;
			_eventsList.AppendColumn("Description", DescriptionCellRenderer, "text", EVENT_DESCRIPTION_COL);
			var useRegExCellRender = new Gtk.CellRendererToggle ();
			useRegExCellRender.Activatable = true;
			useRegExCellRender.Toggled += UseRegExCellRender_Toggled;
			_eventsList.AppendColumn("Use Reg Ex", useRegExCellRender, "active", EVENT_USE_REG_EX_COL);
			var PatternCellRenderer = new Gtk.CellRendererText();
			PatternCellRenderer.Editable = true;
			PatternCellRenderer.Edited += PatternCellRenderer_Edited;
			_eventsList.AppendColumn("Pattern", PatternCellRenderer, "text", EVENT_PATTERN_COL);
			var DeleteActionCellRenderer = new Gtk.CellRendererPixbuf ();
			DeleteActionCellRenderer.IconName = "gtk-add";
			DeleteActionCellRenderer.EditingStarted += DeleteActionCellRenderer_EditingStarted;
			_eventsList.AppendColumn ("XXX", DeleteActionCellRenderer);


			//var temp = new Gtk.CellRenderer ();
			var temp = new Gtk.CellRendererPixbuf();


			_listModel = new Gtk.ListStore (typeof(string), typeof(string), typeof(bool), typeof(string));

			_eventsList.Model = _listModel;
		}

		void DeleteActionCellRenderer_EditingStarted (object o, Gtk.EditingStartedArgs args)
		{
			
		}

		void IDCellRenderer_Edited (object o, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
			if (_listModel.GetIter (out iter, new Gtk.TreePath(args.Path)) == true) {
				_listModel.SetValue (iter, EVENT_ID_COL, args.NewText);
			}
		}

		void DescriptionCellRenderer_Edited (object o, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
			if (_listModel.GetIter (out iter, new Gtk.TreePath(args.Path)) == true) {
				_listModel.SetValue (iter, EVENT_DESCRIPTION_COL, args.NewText);
			}
		}
			
		void UseRegExCellRender_Toggled (object o, Gtk.ToggledArgs args)
		{
			Gtk.TreeIter iter;
			if (_listModel.GetIter (out iter, new Gtk.TreePath(args.Path)) == true) {
				var value = (bool)(_listModel.GetValue (iter, EVENT_USE_REG_EX_COL));
				_listModel.SetValue (iter, EVENT_USE_REG_EX_COL, (value == true) ? false : true);
			}
		}

		void PatternCellRenderer_Edited (object o, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
			if (_listModel.GetIter (out iter, new Gtk.TreePath(args.Path)) == true) {
				_listModel.SetValue (iter, EVENT_PATTERN_COL, args.NewText);
			}
		}

		public void Load(EventDefinitions eventDef)
		{
			_descriptionEntry.Text = eventDef.Description;
			foreach (var element in eventDef.Events) {
				AddRowToTable (element.EventID, element.Description, element.UseRegEx, element.Pattern);
			}
		}

		public bool Validate()
		{
			return true;
		}

		public EventDefinitions ValidateAndUnLoad()
		{
			if (Validate () == true) {
				return UnLoad();
			}
			return null;
		}

		private EventDefinitions UnLoad()
		{
			var retVal = new EventDefinitions ();

			retVal.Description = _descriptionEntry.Text;

			Gtk.TreeIter iter;
			if (_listModel.GetIterFirst (out iter) == true) {
				do {
					string id = (string)_listModel.GetValue(iter, 0);
					string description = (string)_listModel.GetValue(iter, 1);
					bool useRegEx = (bool)_listModel.GetValue(iter, 2);
					string pattern = (string)_listModel.GetValue(iter, 3);
					retVal.Events.Add(new EventDefinition(description, id, pattern, useRegEx));
				} while (_listModel.IterNext (ref iter) == true);
			}

			return retVal;
		}

		private void AddRowToTable (string id, string description, bool useRegEx, string pattern)
		{
			_listModel.AppendValues (id, description, useRegEx, pattern);
		}

		protected void OnAddEventButtonClicked (object sender, EventArgs e)
		{
			AddRowToTable ("x", "", false, "");
		}
	}
}

