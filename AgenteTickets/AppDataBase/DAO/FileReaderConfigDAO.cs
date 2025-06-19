using AgenteTickets.AppDataBase.Models;

namespace AgenteTickets.AppDataBase.DAO
{
    public class FileReaderConfigDAO : DAO<FileReaderConfig>
    {
        public static FileReaderConfig Find()
        {
            return Find(1);
        }
    }
}
