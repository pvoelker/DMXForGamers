﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMXCommunication;

namespace DMXEngine
{
    internal class ActiveEvent
    {
        public ActiveEvent(DateTime start, int iterCount, bool continuous)
        {
            Start = start;
            Iteration = iterCount;
            Continuous = continuous;
        }

        public DateTime Start { get; set; }
        public int Iteration { get; set; }
        public bool Continuous { get; set; }
    }

    public class DMXStateMachine : IDisposable
    {
        private DMX _dmx;
        private IDMXCommunication _dmxComm;
        private Dictionary<string, ActiveEvent> _activeEvents = new Dictionary<string, ActiveEvent>();
        private bool _disposing = false;

        public DMXStateMachine(DMX dmx, IDMXCommunication dmxComm)
        {
            if (dmx == null)
                throw new ArgumentNullException("dmx");
            if (dmxComm == null)
                throw new ArgumentNullException("dmxComm");

            _dmx = dmx;

            _dmxComm = dmxComm;
            _dmxComm.Start();
        }

        public void Execute(DateTime dt)
        {
            if (_disposing == false)
            {

                List<string> finishedEvents = new List<string>();

                lock (_activeEvents)
                {
                    foreach (var element in _activeEvents)
                    {
                        TimeSpan ts = dt - element.Value.Start;

                        var foundEvent = _dmx.Events.Find(x => String.Compare(x.ID, element.Key, true) == 0);
                        if ((foundEvent != null) && ((int)ts.TotalMilliseconds > foundEvent.TimeSpan))
                        {
                            if (element.Value.Iteration > 1)
                            {
                                element.Value.Start = dt;
                                element.Value.Iteration--;
                            }
                            else
                            {
                                if (element.Value.Continuous == true)
                                {
                                    element.Value.Start = dt;
                                    element.Value.Iteration = foundEvent.RepeatCount;
                                }
                                else
                                {
                                    finishedEvents.Add(element.Key);
                                }
                            }
                        }
                    }

                    foreach (var element in finishedEvents)
                    {
                        _activeEvents.Remove(element);
                    }

                    finishedEvents = null;

                    Dictionary<ushort, byte> channelValues = new Dictionary<ushort, byte>();

                    foreach (var element in _activeEvents)
                    {
                        TimeSpan ts = dt - element.Value.Start;

                        var foundEvent = _dmx.Events.Find(x => String.Compare(x.ID, element.Key, true) == 0);
                        if ((foundEvent != null) && ((int)ts.TotalMilliseconds <= foundEvent.TimeSpan))
                        {
                            var eventEnum = foundEvent.TimeBlocks.GetEnumerator();
                            while (eventEnum.MoveNext() == true)
                            {
                                if ((int)ts.TotalMilliseconds > eventEnum.Current.StartTime)
                                {
                                    foreach (var val in eventEnum.Current.DMXValues)
                                    {
                                        short value = val.Value;
                                        if (val.Delta != 0)
                                        {
                                            value += (short)(val.Delta * ((ts.TotalMilliseconds - eventEnum.Current.StartTime) / eventEnum.Current.TimeSpan));
                                        }
                                        channelValues[val.Channel] = (byte)value;
                                    }
                                }
                            }
                        }
                    }

                    foreach (var value in channelValues)
                    {
                        _dmxComm.SetChannelValue(value.Key, value.Value);
                    }

                    // Set base values if no time blocks active
                    if (_activeEvents.Count == 0)
                    {
                        foreach (var val in _dmx.BaseDMXValues)
                        {
                            _dmxComm.SetChannelValue(val.Channel, (byte)val.Value);
                        }
                    }
                }
            }
        }

        public void AddEvent(string eventName, bool continuous = false)
        {
            RemoveEvent(eventName);

            lock (_activeEvents)
            {
                var foundEvent = _dmx.Events.Find(x => String.Compare(x.ID, eventName, true) == 0);
                if (foundEvent != null)
                {
                    _activeEvents.Add(eventName, new ActiveEvent(DateTime.Now, foundEvent.RepeatCount, continuous));
                }
            }
        }

        public void RemoveEvent(string eventName)
        {
            lock (_activeEvents)
            {
                var foundEvent = _dmx.Events.Find(x => String.Compare(x.ID, eventName, true) == 0);
                if (foundEvent != null)
                {
                    _activeEvents.Remove(eventName);
                }
            }
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DMXStateMachine()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            _disposing = true;

            if (disposing)
            {
                // Free managed resources
                if (_dmxComm != null)
                {
                    _dmxComm.Dispose();
                    _dmxComm = null;
                }
            }

            // Free native resources if there are any
        }

        #endregion
    }
}
