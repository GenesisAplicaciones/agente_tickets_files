using ACTools.NetFramework.Controls;
using AgenteTickets.APISelfbilling.Services;
using AgenteTickets.AppDataBase.DAO;
using AgenteTickets.AppDataBase.Enums;
using AgenteTickets.ClientData;
using AgenteTickets.ClientData.Models;
using AgenteTickets.Models;
using AgenteTickets.Persistence;
using log4net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace AgenteTickets.Tasks
{
    public static class TicketTask
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void UploadTickets(LoaderWindow loading = null)
        {
            if (loading != null)
            {
                loading.ModalText = "Buscando tickets...";
            }

            Stopwatch timeMeasure = new Stopwatch();
            timeMeasure.Start();

            log.Info($"Inicio carga automática tickets.");

            List<DataInfo> data = TicketDAO.Get(AppInstances.FileReaderConfig.FileType, AppInstances.FileReaderConfig.Path, AppInstances.SelfbillingConfig.Serie, AppInstances.PaymentMethodsConfig);

            log.Info($"Total tickets: {data.Count}.");

            for (int i = 0; i < data.Count; i++)
            {
                string logText = $"Cargando tickets. {i + 1} de {data.Count}.";
                log.Info(logText);

                if (loading != null)
                {
                    loading.ModalText = $"{logText}..";
                }

                if (data[i].Code.HasValue)
                {
                    TicketDAO.Move(data[i], AppInstances.FileReaderConfig.ResponsePath);
                    _ = TicketLogDAO.Create(data[i].Code, data[i].Message, TypeMethod.AUTOMATIC, TypeAction.LOAD);
                    log.Error($"APP: {data[i].Code}. {data[i].Message}");
                    continue;
                }

                Ticket ticket = data[i].Ticket;

                SelfbillingResponse<TicketMetadata> response = TicketService.UploadTicket(ticket);
                _ = TicketLogDAO.Create(ticket, response, TypeMethod.AUTOMATIC, TypeAction.LOAD);

                data[i].Code = response.Code;
                TicketDAO.Move(data[i], AppInstances.FileReaderConfig.ResponsePath);
            }

            log.Info($"Tiempo: {timeMeasure.Elapsed:G}");
            log.Info($"Precisión: {1.0 / Stopwatch.Frequency:E} segundos");
            log.Info("Fin carga automática tickets.");
        }

        public static void UploadCancellations()
        {
            Stopwatch timeMeasure = new Stopwatch();
            timeMeasure.Start();

            log.Info($"Inicio carga automática cancelaciones.");

            List<DataInfo> data = new List<DataInfo>();

            log.Info($"Total cancelaciones: {data.Count}");

            for (int i = 0; i < data.Count; i++)
            {
                log.Info($"Cargando cancelaciones. {i + 1} de {data.Count}.");

                if (data[i].Code.HasValue)
                {
                    TicketDAO.Move(data[i], AppInstances.FileReaderConfig.ResponsePath);
                    log.Error($"APP: {data[i].Code}. {data[i].Message}");
                    continue;
                }

                Ticket ticket = data[i].Ticket;

                SelfbillingResponse<TicketMetadata> response = TicketService.CancelTicket(ticket);
                _ = TicketLogDAO.Create(ticket, response, TypeMethod.AUTOMATIC, TypeAction.CANCELLATION);

                data[i].Code = response.Code;
                TicketDAO.Move(data[i], AppInstances.FileReaderConfig.ResponsePath);
            }

            log.Info($"Tiempo: {timeMeasure.Elapsed:G}");
            log.Info($"Precisión: {1.0 / Stopwatch.Frequency:E} segundos");
            log.Info($"Fin carga automática cancelaciones.");
        }

        public static void UploadPendingTickets()
        {
            Stopwatch timeMeasure = new Stopwatch();
            timeMeasure.Start();

            log.Info("Inicio carga tickets pendientes.");

            List<DataInfo> data = TicketDAO.GetPending(AppInstances.FileReaderConfig.FileType, AppInstances.FileReaderConfig.ResponsePath, AppInstances.SelfbillingConfig.Serie, AppInstances.PaymentMethodsConfig);

            log.Info($"Total tickets: {data.Count}.");

            for (int i = 0; i < data.Count; i++)
            {
                string logText = $"Cargando tickets. {i + 1} de {data.Count}.";
                log.Info(logText);

                if (data[i].Code.HasValue)
                {
                    TicketDAO.Move(data[i], AppInstances.FileReaderConfig.ResponsePath);
                    _ = TicketLogDAO.Create(data[i].Code, data[i].Message, TypeMethod.AUTOMATIC, TypeAction.LOAD);
                    log.Error($"APP: {data[i].Code}. {data[i].Message}");
                    continue;
                }

                Ticket ticket = data[i].Ticket;
                SelfbillingResponse<TicketMetadata> response = TicketService.UploadTicket(ticket);

                if (!response.Code.HasValue)
                {
                    continue;
                }

                _ = TicketLogDAO.Create(ticket, response, TypeMethod.AUTOMATIC, TypeAction.LOAD);
                data[i].Code = response.Code;
                TicketDAO.Move(data[i], AppInstances.FileReaderConfig.ResponsePath);
            }

            log.Info($"Tiempo: {timeMeasure.Elapsed:G}");
            log.Info($"Precisión: {1.0 / Stopwatch.Frequency:E} segundos");
            log.Info("Fin carga automática tickets.");

            log.Info($"Fin carga tickets pendientes.");
        }
    }
}
