using System;
using System.Diagnostics;

namespace DMXEngine
{
	public class NewProcessTextMonitor : ITextMonitor
	{
		private string _fileName = null;
		private string _arguments = null;
		private Action<string> _processorAction = null;
		private ThreadedProcessingQueue<string> _queue = null;

		private Process _process = null;

		public NewProcessTextMonitor (string fileName, string arguments, Action<string> processor)
		{
			_fileName = fileName;
			_arguments = arguments;
			_processorAction = processor;

			_queue = new ThreadedProcessingQueue<string>(_processorAction);
		}

		public void Start ()
		{
			_queue.Start ();

			_process = new Process {
				StartInfo = {
					FileName = _fileName,
					Arguments = _arguments,
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true
				}
			};
			_process.OutputDataReceived += _process_OutputDataReceived;
			_process.Start ();
			_process.BeginOutputReadLine ();				
		}

		void _process_OutputDataReceived (object sender, DataReceivedEventArgs e)
		{
			_queue.AddToQueue (e.Data);
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		~NewProcessTextMonitor ()
		{
			Dispose (false);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (disposing) {
				// Free managed resources
				if (_process != null) {
					_process.OutputDataReceived -= _process_OutputDataReceived;
					_process.Kill ();
					_process.WaitForExit ();
					_process.Dispose ();
					_process = null;
				}

				if (_queue != null) {
					_queue.Dispose ();
					_queue = null;
				}
			}

			// Free native resources if there are any
		}

		#endregion
	}
}

