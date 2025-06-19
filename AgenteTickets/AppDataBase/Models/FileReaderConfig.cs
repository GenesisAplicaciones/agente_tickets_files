using ACTools.NetFramework.Models;
using AgenteTickets.ClientData.Enums;

namespace AgenteTickets.AppDataBase.Models
{
    public class FileReaderConfig : Model
    {
        public virtual int Id { get; set; }

        private string path;
        public virtual string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged();
            }
        }

        private FileType fileType;
        public virtual FileType FileType
        {
            get => fileType;
            set
            {
                fileType = value;
                OnPropertyChanged();
            }
        }

        private string responsePath;
        public virtual string ResponsePath
        {
            get => responsePath;
            set
            {
                responsePath = value;
                OnPropertyChanged();
            }
        }
    }
}
