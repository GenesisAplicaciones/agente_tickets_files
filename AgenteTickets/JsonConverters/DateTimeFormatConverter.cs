using Newtonsoft.Json.Converters;

namespace AgenteTickets.JsonConverters
{
    public class DateTimeFormatConverter : IsoDateTimeConverter
    {
        public DateTimeFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
