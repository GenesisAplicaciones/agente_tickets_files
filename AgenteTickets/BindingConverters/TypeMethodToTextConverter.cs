using AgenteTickets.AppDataBase.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AgenteTickets.BindingConverters
{
    public class TypeMethodToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TypeMethod type)
            {
                switch (type)
                {
                    case TypeMethod.AUTOMATIC:
                        return "Automático";
                    case TypeMethod.MANUAL:
                        return "Manual";
                    case TypeMethod.NONE:
                    default:
                        return "";
                }
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                switch (text)
                {
                    case "Automático":
                        return TypeMethod.AUTOMATIC;
                    case "Manual":
                        return TypeMethod.MANUAL;
                    default:
                        return TypeMethod.NONE;
                }
            }

            return TypeMethod.NONE;
        }
    }
}
