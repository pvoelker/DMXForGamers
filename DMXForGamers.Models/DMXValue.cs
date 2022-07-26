using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXValue : ObservableValidator
    {
        public DMXValue()
        {
            ValidateAllProperties();
        }

        public IEnumerable<DMXValue> ParentCollection { get; set; }

        private ushort _channel;
        [Range(1, 512,
            ErrorMessage = "DMX Channels Must Be Between 1 and 512 Inclusive")]
        [CustomValidation(typeof(DMXValue), nameof(ValidateChannel))]
        public ushort Channel
        {
            get => _channel;
            set => SetProperty(ref _channel, value, true);
        }
        static public ushort MinChannel { get { return 1; } }
        static public ushort MaxChannel { get { return 512; } }

        private byte _value;
        [CustomValidation(typeof(DMXValue), nameof(ValidateDeltaAndValueTotal))]
        public byte Value
        {
            get => _value;
            set => SetProperty(ref _value, value, true);
        }
        static public short MinValue { get { return 0; } }
        static public short MaxValue { get { return 255; } }

        private short _delta;
        [Range(-255, 255,
            ErrorMessage = "Delta Must Be Between -255 and 255 Inclusive")]
        [CustomValidation(typeof(DMXValue), nameof(ValidateDeltaAndValueTotal))]
        public short Delta
        {
            get => _delta;
            set => SetProperty(ref _delta, value, true);
        }
        static public short MinDelta { get { return -255; } }
        static public short MaxDelta { get { return 255; } }

        private ICommand _deleteDMXValue;
        public ICommand DeleteDMXValue
        {
            get => _deleteDMXValue;
            set => SetProperty(ref _deleteDMXValue, value, true);
        }

        #region Custom validation

        public static ValidationResult ValidateChannel(string name, ValidationContext context)
        {
            var instance = (DMXValue)context.ObjectInstance;

            if (instance.ParentCollection != null)
            {
                var duplicateCount = instance.ParentCollection.Where(x => x != instance)
                    .Count(x => x.Channel == instance.Channel);
                if (duplicateCount > 0)
                {
                    return new ValidationResult("Channel is duplicated");
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateDeltaAndValueTotal(string name, ValidationContext context)
        {
            var instance = (DMXValue)context.ObjectInstance;

            if (instance.ParentCollection != null)
            {
                var total = instance.Value + instance.Delta;
                if ((total < MinValue) || (total > MaxValue))
                {
                    return new ValidationResult(String.Format("Total of Value and Delta Values Must Be Between {0} and {1} Inclusive", MinValue, MaxValue));
                }
            }

            return ValidationResult.Success;
        }

        #endregion

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            return errors;
        }
    }
}
