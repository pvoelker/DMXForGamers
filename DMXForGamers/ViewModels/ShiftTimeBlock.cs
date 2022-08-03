using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DMXForGamers.ViewModels
{
    public class ShiftTimeBlock : ObservableValidator
    {
        private int _startTime;
        public int StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        private int _timeSpan;
        public int TimeSpan
        {
            get => _timeSpan;
            set => SetProperty(ref _timeSpan, value);
        }

        private int? _newStartTime;
        [Range(0, int.MaxValue,
            ErrorMessage = "New Start Time must be equal to or greater than 0")]
        public int? NewStartTime
        {
            get => _newStartTime;
            set => SetProperty(ref _newStartTime, value, true);
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            return errors;
        }
    }
}
