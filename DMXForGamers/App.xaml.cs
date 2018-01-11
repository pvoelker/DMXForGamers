using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DMXForGamers
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
#endif

            base.OnStartup(e);
        }
    }

#if DEBUG
    public class DebugTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            // Do nothing
        }

        public override void WriteLine(string message)
        {
            Debugger.Break();
        }
    }
#endif
}
