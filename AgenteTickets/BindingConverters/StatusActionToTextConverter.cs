using AgenteTickets.AppDataBase.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AgenteTickets.BindingConverters
{
    public class StatusActionToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatusAction type)
            {
                switch (type)
                {
                    case StatusAction.COMPLETED:
                        return "Completado";
                    case StatusAction.NOT_COMPLETED:
                        return "NO completado";
                    case StatusAction.NONE:
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
                    case "Completado":
                        return StatusAction.COMPLETED;
                    case "NO completado":
                        return StatusAction.NOT_COMPLETED;
                    default:
                        return StatusAction.NONE;
                }
            }

            return StatusAction.NONE;
        }
    }
}
