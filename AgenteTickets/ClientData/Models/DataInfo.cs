using AgenteTickets.Models;

namespace AgenteTickets.ClientData.Models
{
    public class DataInfo
    {
        public string NewFileName { get; set; }
        public string PathFileName { get; set; }
        public string FileExtension { get; set; }
        public Ticket Ticket { get; set; }
        public int? Code { get; set; }
        public string Message { get; set; }
    }
}
