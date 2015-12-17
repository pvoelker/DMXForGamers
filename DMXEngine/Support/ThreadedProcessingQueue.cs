using System;
using System.Collections.Generic;
using System.Threading;

namespace DMXEngine
{
	class ThreadedProcessingQueue<T> : IDisposable
	{
		private Action<T> _processor = null;
		private Queue<T> _queue = null;
		private Thread _processingThread = null;
		private ManualResetEventSlim _processingWait = new ManualResetEventSlim (false);
		private ReaderWriterLockSlim _queueLock = new ReaderWriterLockSlim ();

		public ThreadedProcessingQueue (Action<T> processor)
		{
			_processor = processor;
		}

		public void Start (ThreadPriority processingPriority = ThreadPriority.Normal)
		{
			_queue = new Queue<T> ();

			var threadStart = new ThreadStart (ProcessQueue);
			_processingThread = new Thread (threadStart);
			_processingThread.Name = "Queue Processing - " + this.GetHashCode ();
			_processingThread.Priority = processingPriority;
			_processingThread.Start ();
		}

		public void AddToQueue (T value)
		{
			if (_queue != null) {
				try {
					_queueLock.EnterWriteLock ();
					if (_queue != null) {
						_queue.Enqueue (value);
					}
				} finally {
					_queueLock.ExitWriteLock ();
				}
			}
			_processingWait.Set ();
		}

		private void ProcessQueue ()
		{
			while (_queue != null) {
				_processingWait.Wait ();
				while (true) {
					if (_queue != null) {
						try {
							_queueLock.EnterReadLock ();
							if (_queue != null) {
								if (_queue.Count > 0) {
									_processor (_queue.Dequeue ());
									Thread.Sleep(0);
								} else {
									// Queue empty, go into wait state
									break;
								}
							}
						} finally {
							_queueLock.ExitReadLock ();
						}
					} else {
						// End thread (_queue == null)
						break;
					}
				}
				_processingWait.Reset ();
			}           
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);          
		}

		protected virtual void Dispose (bool disposing)
		{
			try {
				if (_queue != null) {
					_queueLock.EnterWriteLock ();
					if (_queue != null) {
						_queue = null;
					}
				}
			} finally {
				_queueLock.ExitWriteLock ();
			}

			_processingWait.Set ();

			if (_processingThread != null) {
				_processingThread.Join ();
				_processingThread = null;
			}

			if (disposing) {
				// Free any other managed objects here
			}

			// Free any unmanaged objects here
		}

		~ThreadedProcessingQueue ()
		{
			Dispose (false);
		}

		#endregion
	}
}

