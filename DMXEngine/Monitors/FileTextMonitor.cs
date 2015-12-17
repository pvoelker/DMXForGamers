using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DMXEngine
{
	public class FileTextMonitor : ITextMonitor
	{
		private string _filePathName = null;
		private FileSystemWatcher _fileWatcher = null;
		private long _lastFileSize = 0;
		private DateTime _lastFileModified = DateTime.MinValue;
		private Action<string> _processorAction = null;

		public FileTextMonitor(string file, Action<string> processor)
		{
			_filePathName = file;
			_processorAction = processor;
		}

		public void Start()
		{
			var fileName = Path.GetFileName(_filePathName);
			var fileDirectory = Path.GetDirectoryName(_filePathName);

			try
			{
				_fileWatcher = new FileSystemWatcher(fileDirectory, fileName);
				_fileWatcher.Changed += _fileWatcher_Changed;
				_fileWatcher.Created += _fileWatcher_Changed;
				_fileWatcher.Deleted += _fileWatcher_Changed;
				_fileWatcher.Renamed += _fileWatcher_Changed;
				//_fileWatcher.Filter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
				_fileWatcher.EnableRaisingEvents = true;

				var fileInfo = new FileInfo(_filePathName);
				_lastFileModified = fileInfo.LastWriteTime;
				_lastFileSize = fileInfo.Length;
			}
			catch(Exception ex) {
				string temp = ex.Message;
			}
		}

		void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath == _filePathName)
			{
				int i = 0;
				while(i < 5) // Try 5 times to open file
				{
					try
					{
						var fileInfo = new FileInfo(_filePathName);
						if (fileInfo.LastWriteTime > _lastFileModified)
						{
							FileStream fs = null;
							try
							{
								fs = new FileStream(_filePathName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

								using (StreamReader sr = new StreamReader(fs, true))
								{
									fs = null;

									if (fileInfo.Length < _lastFileSize)
									{
										// Assume a new or recreated file, start reading from the beginning
										_lastFileSize = 0;
									}
									else
									{
										sr.BaseStream.Seek(_lastFileSize, SeekOrigin.Begin);
									}

									string str = sr.ReadToEnd();
									var lines = Regex.Split(str, "\r\n|\r|\n");

									foreach (string line in lines)
									{
										_processorAction(line);
									}

									_lastFileSize = fileInfo.Length;

									break;
								}
							}
							finally
							{
								if(fs != null)
									fs.Dispose();
							}
						}
					}
					catch
					{
						// Pause before trying to open file again
						Thread.Sleep(20);
					}
				}
			}
		}

		#region IDisposable implementation

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Free any other managed objects here
				if (_fileWatcher != null)
				{
					_fileWatcher.Changed -= _fileWatcher_Changed;
					_fileWatcher.Created -= _fileWatcher_Changed;
					_fileWatcher.Deleted -= _fileWatcher_Changed;
					_fileWatcher.Renamed -= _fileWatcher_Changed;
					_fileWatcher.Dispose();
					_fileWatcher = null;
				}
			}

			// Free any unmanaged objects here.
		}

		~FileTextMonitor()
		{
			Dispose(false);
		}

		#endregion
	}
}
