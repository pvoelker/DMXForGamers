using DMXCommunication;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Providers;
using SoundFlow.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DMXEngine
{
    internal class ActiveEvent
    {
        public ActiveEvent(DateTime start, int iterCount, bool continuous, SoundPlayer player)
        {
            Start = start;
            Iteration = iterCount;
            Continuous = continuous;
            Player = player;
        }

        public DateTime Start { get; set; }
        public int Iteration { get; set; }
        public bool Continuous { get; set; }
        public SoundPlayer Player { get; set; }
    }

    public class DMXChannelChange
    {
        public DMXChannelChange(ushort channel, byte value)
        {
            Channel = channel;
            Value = value;
        }

        public ushort Channel { get; set; }
        public byte Value { get; set; }
    }

    public class EventChange
    {
        public EventChange(string eventID, bool state, TimeSpan? executionTime)
        {
            EventID = eventID;
            State = state;
            ExecutionTime = executionTime;
        }

        public string EventID { get; set; }
        public bool State { get; set; }
        public TimeSpan? ExecutionTime { get; set; }
    }

    public class DMXStateMachine : IDisposable
    {
        private static readonly MiniAudioEngine _audioEngine = new MiniAudioEngine();
        private static readonly AudioFormat _audioFormat = AudioFormat.DvdHq;

        private bool _disposing = false;

        private AudioPlaybackDevice _playbackDevice = null;

        private readonly DMX _dmx;
        private readonly Dictionary<string, int> _eventTotalTimeSpans;
        private readonly Dictionary<string, TimeSpan?> _lastExecTimeUpdate;
        private readonly Dictionary<string, ActiveEvent> _activeEvents = new Dictionary<string, ActiveEvent>();

        private IDMXCommunication _dmxComm;
        private ThreadedProcessingQueue<DMXChannelChange> _channelChangeQueue;
        private ThreadedProcessingQueue<EventChange> _eventChangeQueue;

        public DMXStateMachine(DMX dmx, IDMXCommunication dmxComm, Action<DMXChannelChange> channelChange, Action<EventChange> eventChange)
        {
            ArgumentNullException.ThrowIfNull(dmx);
            ArgumentNullException.ThrowIfNull(dmxComm);

            _dmx = dmx;
            _eventTotalTimeSpans = _dmx.Events.Select(x => new Tuple<string, int>(x.EventID, x.TimeBlocks.Max(x2 => x2.StartTime + x2.TimeSpan))).ToDictionary(x => x.Item1, x => x.Item2);
            _lastExecTimeUpdate = _dmx.Events.Select(x => new Tuple<string, TimeSpan?>(x.EventID, null)).ToDictionary(x => x.Item1, x => x.Item2);

            if (channelChange != null)
            {
                _channelChangeQueue = new ThreadedProcessingQueue<DMXChannelChange>(channelChange);
                _channelChangeQueue.Start();
            }

            if (eventChange != null)
            {
                _eventChangeQueue = new ThreadedProcessingQueue<EventChange>(eventChange);
                _eventChangeQueue.Start();
            }

            _dmxComm = dmxComm;
            _dmxComm.Start();

            _audioEngine.UpdateDevicesInfo();
            var deviceInfo = _audioEngine.PlaybackDevices.SingleOrDefault(x => x.IsDefault);
            if (deviceInfo != default(DeviceInfo))
            {
                _playbackDevice = _audioEngine.InitializePlaybackDevice(deviceInfo, _audioFormat);
                _playbackDevice.Start();
            }
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

                        var foundEvent = _dmx.Events.Find(x => String.Compare(x.EventID, element.Key, true) == 0);
                        if ((foundEvent != null) && ((int)ts.TotalMilliseconds > _eventTotalTimeSpans[element.Key]))
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

                        if (_eventChangeQueue != null)
                        {
                            _eventChangeQueue.AddToQueue(new EventChange(element, false, null));
                            _lastExecTimeUpdate[element] = null;
                        }
                    }

                    finishedEvents = null;

                    var channelValues = new Dictionary<ushort, byte>();

                    foreach (var element in _activeEvents)
                    {
                        TimeSpan ts = dt - element.Value.Start;

                        var foundEvent = _dmx.Events.Find(x => String.Compare(x.EventID, element.Key, true) == 0);
                        if ((foundEvent != null) && ((int)ts.TotalMilliseconds <= _eventTotalTimeSpans[element.Key]))
                        {
                            if (element.Value.Player != null)
                            {
                                // Throttle time updates to every 100 ms
                                var last = _lastExecTimeUpdate[foundEvent.EventID];
                                Debug.Assert(last.HasValue, "This should always have a value because the event is active");
                                var current = TimeSpan.FromSeconds(element.Value.Player.Time);
                                if ((current - last.Value).TotalMilliseconds >= 100)
                                {
                                    _eventChangeQueue.AddToQueue(new EventChange(foundEvent.EventID, true, current));
                                    _lastExecTimeUpdate[foundEvent.EventID] = current;
                                }
                            }

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

                        _channelChangeQueue?.AddToQueue(new DMXChannelChange(value.Key, value.Value));
                    }

                    // Set base values if no time blocks active
                    if (_activeEvents.Count == 0)
                    {
                        foreach (var val in _dmx.BaseDMXValues)
                        {
                            _dmxComm.SetChannelValue(val.Channel, (byte)val.Value);

                            _channelChangeQueue?.AddToQueue(new DMXChannelChange(val.Channel, (byte)val.Value));
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
                var foundEvent = _dmx.Events.Find(x => String.Compare(x.EventID, eventName, true) == 0);
                if (foundEvent != null)
                {
                    if (_dmx.AllowOneActiveEvent)
                    {
                        ClearAllActiveEvents();
                    }

                    // Start sound first before lighting event so the timing will be more consistent
                    SoundPlayer player = null;
                    if (foundEvent.SoundData != null && foundEvent.SoundData.Length > 0)
                    {
                        var provider = new StreamDataProvider(_audioEngine, _audioFormat, new MemoryStream(foundEvent.SoundData));
                        player = new SoundPlayer(_audioEngine, _audioFormat, provider);

                        _playbackDevice?.MasterMixer.AddComponent(player);

                        player.Play();
                    }

                    _activeEvents.Add(eventName, new ActiveEvent(DateTime.Now, foundEvent.RepeatCount, continuous, player));

                    if (_eventChangeQueue != null)
                    {
                        _eventChangeQueue.AddToQueue(new EventChange(eventName, true, new TimeSpan(0)));
                        _lastExecTimeUpdate[eventName] = new TimeSpan(0);
                    }
                }
            }
        }

        public void RemoveEvent(string eventName)
        {
            lock (_activeEvents)
            {
                var foundEvent = _dmx.Events.Find(x => String.Compare(x.EventID, eventName, true) == 0);
                if (foundEvent != null)
                {
                    ActiveEvent activeEvent = null;
                    _activeEvents.TryGetValue(eventName, out activeEvent);

                    if (activeEvent != null)
                    {
                        if (activeEvent.Player != null)
                        {
                            CleanupPlayer(activeEvent.Player);
                            activeEvent.Player = null;
                        }

                        _activeEvents.Remove(eventName);
                    }

                    if (_eventChangeQueue != null)
                    {
                        _eventChangeQueue.AddToQueue(new EventChange(eventName, false, null));
                        _lastExecTimeUpdate[eventName] = null;
                    }
                }
            }
        }

        private void ClearAllActiveEvents()
        {
            foreach (var element in _activeEvents)
            {
                if (element.Value.Player != null)
                {
                    CleanupPlayer(element.Value.Player);
                    element.Value.Player = null;
                }

                if (_eventChangeQueue != null)
                {
                    _eventChangeQueue.AddToQueue(new EventChange(element.Key, false, null));
                    _lastExecTimeUpdate[element.Key] = null;
                }
            }
            _activeEvents.Clear();
        }

        private void CleanupPlayer(SoundPlayer player)
        {
            player.Stop();
            _playbackDevice?.MasterMixer.RemoveComponent(player);
            player.Dispose();
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
                if (_channelChangeQueue != null)
                {
                    for (ushort i = 1; i <= 512; i++)
                    {
                        _channelChangeQueue.AddToQueue(new DMXChannelChange(i, 0));
                    }
                }

                if(_playbackDevice != null)
                {
                    _playbackDevice.Stop();
                    _playbackDevice.Dispose();
                    _playbackDevice = null;
                }

                // Free managed resources
                if (_dmxComm != null)
                {
                    _dmxComm.Dispose();
                    _dmxComm = null;
                }

                if(_channelChangeQueue != null)
                {
                    _channelChangeQueue.Dispose();
                    _channelChangeQueue = null;
                }

                if (_eventChangeQueue != null)
                {
                    _eventChangeQueue.Dispose();
                    _eventChangeQueue = null;
                }
            }

            // Free native resources if there are any
        }

        #endregion
    }
}
