using System;
using System.ComponentModel;
using System.Net;

namespace DMXCommunication.Converters
{
    /// <summary>
    /// Type converter to convert between <see cref="string"/> representations and <see cref="System.Net.IPAddress"/> objects  
    /// </summary>
    internal class IPAddressConverter : TypeConverter
    {
        /// <summary>
        /// Determines whether this converter can convert an object of the specified source type to the type of this converter
        /// </summary>
        /// <param name="context">Optional format context</param>
        /// <param name="sourceType">Type of the source object for conversion</param>
        /// <returns>True if the converter can perform the conversion, otherwise false</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the specified value to an IPAddress object
        /// </summary>
        /// <param name="context">Optional format context</param>
        /// <param name="culture">Optional culture information to use for conversion</param>
        /// <param name="value">The value to convert. Typically a string representation of an IP address</param>
        /// <returns>An IPAddress object that represents the converted value</returns>
        /// <exception cref="ArgumentNullException">Thrown if the value is null</exception>
        /// <exception cref="FormatException">Thrown if the value is not a valid IP address string</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string strValue)
                return IPAddress.Parse(strValue);
            return base.ConvertFrom(context, culture, value);
        }
    }
}
