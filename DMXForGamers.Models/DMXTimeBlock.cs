using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXTimeBlock : NotifyPropertyChangedWithErrorInfoBase
    {
        public DMXTimeBlock()
        {
            DMXValues = new ObservableCollection<DMXValue>();

            AddDMXValue = new RelayCommand(x =>
            {
                DMXValues.Add(new DMXValue());
            });

            SortDMXValues = new RelayCommand(x =>
            {
                DMXValues = new ObservableCollection<DMXValue>(DMXValues.OrderBy(y => y.Channel));
            });
        }

        private int _startTime;
        public int StartTime
        {
            get { return _startTime; }
            set { _startTime = value; AnnouncePropertyChanged(); }
        }

        private int _timeSpan;
        public int TimeSpan
        {
            get { return _timeSpan; }
            set { _timeSpan = value; AnnouncePropertyChanged(); }
        }

        private ObservableCollection<DMXValue> _dmxValues;
        public ObservableCollection<DMXValue> DMXValues
        {
            get { return _dmxValues; }
            set
            {
                if (_dmxValues != null)
                {
                    foreach (var item in _dmxValues)
                    {
                        var dmxValue = item as DMXValue;
                        dmxValue.DeleteDMXValue = null;
                        dmxValue.ParentCollection = null;
                    }
                    _dmxValues.CollectionChanged -= _dmxValues_CollectionChanged;
                }
                _dmxValues = value;
                if (_dmxValues != null)
                {
                    foreach (var item in _dmxValues)
                    {
                        var dmxValue = item as DMXValue;
                        dmxValue.DeleteDMXValue = new RelayCommand(x => _dmxValues.Remove((x as DMXValue)));
                        dmxValue.ParentCollection = this.DMXValues;
                    }
                    _dmxValues.CollectionChanged += _dmxValues_CollectionChanged;
                }
                AnnouncePropertyChanged();
            }
        }

        private void _dmxValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var dmxValue = item as DMXValue;
                    dmxValue.DeleteDMXValue = null;
                    dmxValue.ParentCollection = null;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var dmxValue = item as DMXValue;
                    dmxValue.DeleteDMXValue = new RelayCommand(x => _dmxValues.Remove((x as DMXValue)));
                    dmxValue.ParentCollection = this.DMXValues;
                }
            }
        }

        private ICommand _addDMXValue;
        public ICommand AddDMXValue
        {
            get { return _addDMXValue; }
            set { _addDMXValue = value; AnnouncePropertyChanged(); }
        }

        private ICommand _deleteTimeBlock;
        public ICommand DeleteTimeBlock
        {
            get { return _deleteTimeBlock; }
            set { _deleteTimeBlock = value; AnnouncePropertyChanged(); }
        }

        private ICommand _sortDMXValues;
        public ICommand SortDMXValues
        {
            get { return _sortDMXValues; }
            set { _sortDMXValues = value; AnnouncePropertyChanged(); }
        }

        #region IErrorInfo

        public override string this[string columnName]
        {
            get
            {
                var errorStr = new StringBuilder();

                if ((columnName == nameof(StartTime)) || (columnName == null))
                {
                    if (StartTime < 0)
                    {
                        errorStr.AppendLine("Start Time must be greater than or equal to 0");
                    }
                }
                if ((columnName == nameof(TimeSpan)) || (columnName == null))
                {
                    if (TimeSpan <= 0)
                    {
                        errorStr.AppendLine("Time Span must be greater than 0");
                    }
                }

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion

        override public IEnumerable<string> Validate()
        {
            var errors = new List<string>();            

            errors.AddRange(Errors);

            #region Children

            errors.AddRange(DMXValues.SelectMany(x => x.Validate().
                Select(y => String.Format("DMX Channel {0} - {1}", x.Channel, y))));

            #endregion

            return errors;
        }
    }
}
