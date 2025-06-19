using AgenteTickets.AppDataBase.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AgenteTickets.BindingConverters
{
    public class TypeActionToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TypeAction type)
            {
                switch (type)
                {
                    case TypeAction.LOAD:
                        return "Carga";
                    case TypeAction.CANCELLATION:
                        return "Cancelación";
                    case TypeAction.NONE:
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
                    case "Carga":
                        return TypeAction.LOAD;
                    case "Cancelación":
                        return TypeAction.CANCELLATION;
                    default:
                        return TypeAction.NONE;
                }
            }

            return TypeAction.NONE;
        }
    }
}
