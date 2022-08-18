using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DMXForGamers.Models
{
    public class ShiftTimeBlocks : ObservableValidator
    {
        private int _shiftTimeBy;
        public int ShiftTimeBy
        {
            get => _shiftTimeBy;
            set
            {
                SetProperty(ref _shiftTimeBy, value);
                CalculateNewStartTimes();
            }
        }

        private ShiftTimeBlock _selectedItem;
        public ShiftTimeBlock SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                CalculateNewStartTimes();
            }
        }

        private ObservableCollection<ShiftTimeBlock> _values = new ObservableCollection<ShiftTimeBlock>();
        public ObservableCollection<ShiftTimeBlock> Values
        {
            get { return _values; }
        }

        private void CalculateNewStartTimes()
        {
            foreach (var item in Values)
            {
                item.NewStartTime = null;
            }

            if (SelectedItem != null && ShiftTimeBy != 0)
            {
                foreach (var item in Values.Where(x => x.StartTime >= SelectedItem.StartTime))
                {
                    item.NewStartTime = item.StartTime + ShiftTimeBy;
                }
            }
        }

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            #region Children

            errors.AddRange(Values.SelectMany(x => x.Validate().
                Select(y => String.Format("Time Block '{0}' - {1}", x.StartTime, y))));

            #endregion

            return errors;
        }
    }
}
