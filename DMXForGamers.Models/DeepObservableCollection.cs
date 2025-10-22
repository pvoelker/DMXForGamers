using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace DMXForGamers.Models
{
    public sealed class DeepObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        private readonly HashSet<string> _ignorePropertyNames = new HashSet<string>();

        public DeepObservableCollection(HashSet<string> ignorePropertyNames = null)
        {
            if (ignorePropertyNames != null)
            {
                _ignorePropertyNames = ignorePropertyNames;
            }

            CollectionChanged += DeepObservableCollectionCollectionChanged;
        }

        public DeepObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (var item in pItems)
            {
                this.Add(item);
            }
        }

        private void DeepObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignorePropertyNames.Count == 0 || !_ignorePropertyNames.Contains(e.PropertyName))
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
                OnCollectionChanged(args);
            }
        }
    }
}
