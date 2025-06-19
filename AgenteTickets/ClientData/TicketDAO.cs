using AgenteTickets.AppDataBase.Models;
using AgenteTickets.ClientData.Enums;
using AgenteTickets.ClientData.Models;
using System.Collections.Generic;
using System.IO;

namespace AgenteTickets.ClientData
{
    public class TicketDAO
    {
        public static List<DataInfo> Get(FileType fileType, string path, string serie, List<PaymentMethodConfig> paymentMethodsConfig)
        {
            switch (fileType)
            {
                case FileType.AlohaXML:
                    return AlohaXML.DAO.TicketDAO.Get(path, serie, paymentMethodsConfig);
                case FileType.NONE:
                default:
                    return null;
            }
        }

        public static List<DataInfo> GetPending(FileType fileType, string responsePath, string serie, List<PaymentMethodConfig> paymentMethodsConfig)
        {
            string path = Path.Combine(responsePath, "999");
            _ = Directory.CreateDirectory(path);

            switch (fileType)
            {
                case FileType.AlohaXML:
                    return AlohaXML.DAO.TicketDAO.Get(path, serie, paymentMethodsConfig);
                case FileType.NONE:
                default:
                    return null;
            }
        }

        public static void Move(DataInfo dataInfo, string responsePath)
        {
            int code = dataInfo.Code ?? 999;
            string destFileName = Path.Combine(responsePath, code.ToString());

            if (code == 200)
            {
                destFileName = Path.Combine(destFileName, dataInfo.Ticket.TicketDate.Year.ToString(), dataInfo.Ticket.TicketDate.Month.ToString().PadLeft(2, '0'), dataInfo.Ticket.TicketDate.Day.ToString().PadLeft(2, '0'));
            }

            _ = Directory.CreateDirectory(destFileName);

            destFileName = Path.Combine(destFileName, dataInfo.NewFileName);

            File.Move(dataInfo.PathFileName, GetAvailableFileName(destFileName, dataInfo.FileExtension));
        }

        private static string GetAvailableFileName(string fileName, string fileExtension)
        {
            string newFileName = fileName;
            int count = 0;

            while (File.Exists(newFileName))
            {
                count++;
                newFileName = $"{fileName.Replace(fileExtension, "")} ({count}){fileExtension}";
            }

            return newFileName;
        }
    }
}
