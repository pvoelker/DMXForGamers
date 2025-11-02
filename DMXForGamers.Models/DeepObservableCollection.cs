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
        }

        public DeepObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (var item in pItems)
            {
                this.Add(item);
            }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            AttachItem(item);
        }

        protected override void SetItem(int index, T item)
        {
            var oldItem = this[index];
            DetachItem(oldItem);
            base.SetItem(index, item);
            AttachItem(item);
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            DetachItem(item);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this.ToList())
            {
                DetachItem(item);
            }
            base.ClearItems();
        }

        private void AttachItem(INotifyPropertyChanged item)
        {
            if (item != null)
            {
                item.PropertyChanged += ItemPropertyChanged;
            }
        }

        private void DetachItem(INotifyPropertyChanged item)
        {
            if (item != null)
            {
                item.PropertyChanged -= ItemPropertyChanged;
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
