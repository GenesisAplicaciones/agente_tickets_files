using AgenteTickets.AppDataBase.Models;
using AgenteTickets.ClientData.Enums;
using FluentNHibernate.Mapping;

namespace AgenteTickets.AppDataBase.Maps
{
    public class FileReaderConfigMap: ClassMap<FileReaderConfig>
    {
        public FileReaderConfigMap()
        {
            Table("file_reader_config");
            _ = Id(x => x.Id, "id").Not.Nullable();
            _ = Map(x => x.Path, "path").Not.Nullable();
            _ = Map(x => x.FileType, "file_type").CustomType<FileType>().Not.Nullable();
            _ = Map(x => x.ResponsePath, "response_path").Not.Nullable();
        }
    }
}
