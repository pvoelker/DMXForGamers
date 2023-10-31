using System;
using System.Threading;
using System.Windows;

namespace DMXForGamers
{
    public static class WpfHelpers
    {
        // Make sure action is executed on the UI thread for WPF in order to precent lockups
        public static void InvokeIfNecessary(Action action)
        {
            if (Thread.CurrentThread == Application.Current.Dispatcher.Thread)
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }
    }
}
