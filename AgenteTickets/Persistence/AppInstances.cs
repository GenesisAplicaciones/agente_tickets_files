using AgenteTickets.AppDataBase;
using AgenteTickets.AppDataBase.DAO;
using AgenteTickets.AppDataBase.Models;
using AgenteTickets.Jobs;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AgenteTickets.Persistence
{
    public static class AppInstances
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static SelfbillingConfig SelfbillingConfig { get; set; }
        public static FileReaderConfig FileReaderConfig { get; set; }
        public static List<PaymentMethodConfig> PaymentMethodsConfig { get; set; }
        public static UploadTicketsJob UploadTicketsJob { get; private set; }
        public static UploadCancellationsJob UploadCancellationsJob { get; private set; }
        public static UploadPendingTicketsJob UploadPendingTicketsJob { get; private set; }

        public static bool Initialize(bool retry = false)
        {
            log.Info($"Cargando configuración.");

            bool selfbillingConfigEmpty = false;

            if (SelfbillingConfig == null)
            {
                SelfbillingConfig = SelfbillingConfigDAO.Find();
                selfbillingConfigEmpty = SelfbillingConfig == null || string.IsNullOrEmpty(SelfbillingConfig.ApiUrl);
                log.Info($"Configuración API Autofactura {(selfbillingConfigEmpty ? "sin datos." : "cargada.")}");
            }

            bool fileReaderConfigEmpty = false;

            if (FileReaderConfig == null)
            {
                FileReaderConfig = FileReaderConfigDAO.Find();
                fileReaderConfigEmpty = FileReaderConfig == null || string.IsNullOrEmpty(FileReaderConfig.Path);
                log.Info($"Configuración lector de archivos {(fileReaderConfigEmpty ? "sin datos." : "cargada.")}");
            }

            if (PaymentMethodsConfig == null)
            {
                PaymentMethodsConfig = PaymentMethodConfigDAO.Get();
            }

            if (selfbillingConfigEmpty || fileReaderConfigEmpty)
            {
                log.Info($"{AppDBInstance.DBFileName} corrompido.");

                if (retry)
                {
                    if (AppDBInstance.RestoreDBFile())
                    {
                        log.Info("Copia de seguridad restaurada.");
                        log.Info("Reintentando inicialización.");
                        return Initialize();
                    }
                    else
                    {
                        log.Info("No se pudó restaurar la copia de seguridad.");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            UploadTicketsJob = new UploadTicketsJob();
            UploadTicketsJob.Change(SelfbillingConfig.LoadIntervalTickets, SelfbillingConfig.LoadIntervalTickets);

            UploadCancellationsJob = new UploadCancellationsJob();
            UploadCancellationsJob.Change(SelfbillingConfig.TimeLoadCancellations);

            UploadPendingTicketsJob = new UploadPendingTicketsJob();
            UploadPendingTicketsJob.Change(new TimeSpan(0, 10, 0), new TimeSpan(0, 10, 0));

            log.Info("Jobs iniciados.");

            return true;
        }
    }
}
