using System.IO;
using System.Xml.Serialization;

namespace AgenteTickets.Utils
{
    public class Converter
    {
        public static T XMLToObject<T>(string fileName)
        {
            T value = default;
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                value = (T)serializer.Deserialize(fileStream);
            }

            return value;
        }
    }
}
