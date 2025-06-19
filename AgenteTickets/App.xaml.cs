using ACTools.NetFramework.Application;
using AgenteTickets.AppDataBase;
using AgenteTickets.Persistence;
using AgenteTickets.Tasks;
using AgenteTickets.Views;
using log4net;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace AgenteTickets
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static NotifyApp notification;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Process procesoActual = Process.GetCurrentProcess();

            if ((e.Args.Length == 0 || Convert.ToBoolean(e.Args[0])) && Process.GetProcessesByName(procesoActual.ProcessName).Count() > 1)
            {
                Environment.Exit(0);
            }

            new SplashScreenWindow((splash) =>
            {
                log.Info("Iniciando aplicación.");
                splash.HideModal = true;

                try
                {
                    splash.ModalText = "Iniciando aplicación...";

                    bool make = true;

                    if (!AppDBInstance.DBFileExist)
                    {
                        log.Info("Configuración inicial.");
                        make = false;
                        Current.Dispatcher.Invoke(() =>
                        {
                            splash.Hide();

                            bool saved = new ConfigurationWindow().ShowDialog() ?? false;

                            if (!saved)
                            {
                                log.Info("Aplicación detenida por configuración omitida.");
                                Environment.Exit(0);
                            }

                            splash.Show();
                        });
                    }

                    if (!AppInstances.Initialize(true))
                    {
                        splash.Dispatcher.Invoke(() =>
                        {
                            splash.Hide();
                            _ = MessageBox.Show("No se pudó recuperar la configuración.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        });
                        log.Info("Aplicación detenida por configuración corrupta.");
                        Environment.Exit(0);
                    }

                    if (!Directory.Exists(AppInstances.FileReaderConfig.Path))
                    {
                        splash.Dispatcher.Invoke(() =>
                        {
                            splash.Hide();
                            _ = MessageBox.Show("La ruta de ubación para lectura de archivos no existe. Configure una ruta valida para que la aplicación se pueda ejecutar.", "Agente tickets", MessageBoxButton.OK, MessageBoxImage.Error);

                            bool saved = new ConfigurationWindow().ShowDialog() ?? false;

                            if (!saved)
                            {
                                log.Info("Configuración omitida por el usuario (Ubicación archivos).");
                                Environment.Exit(0);
                            }

                            splash.Show();
                        });
                    }

                    if (AppInstances.FileReaderConfig.FileType == ClientData.Enums.FileType.AlohaXML && !AppInstances.PaymentMethodsConfig.Any())
                    {
                        Current.Dispatcher.Invoke(() =>
                        {
                            splash.Hide();
                            _ = new PaymentMethodConfigWindow().ShowDialog() ?? false;
                            splash.Show();
                        });
                    }

                    if (make)
                    {
                        try
                        {
                            TicketTask.UploadTickets(splash);
                        }
                        catch (SqlException ex)
                        {
                            _ = MessageBox.Show("Se produjo un error al consultar los tickets. Revisa la configuración de la aplicación.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                            log.Error(ex);
                        }
                    }

                    splash.ModalText = "Iniciando aplicación...";

                    AppInstances.UploadTicketsJob.Schedule();
                    //AppInstances.UploadCancellationsJob.Schedule();
                    AppInstances.UploadPendingTicketsJob.Schedule();

                    notification = new NotifyApp(icon: AgenteTickets.Properties.Resources.app);
                    notification.AddOption("Configuración", Configuration_Click);
                    if (AppInstances.FileReaderConfig.FileType == ClientData.Enums.FileType.AlohaXML)
                    {
                        notification.AddOption("Formas de pago", PaymentMethodConfig_Click);
                    }
                    notification.AddOption("Bitácora de tickets", TicketLog_Click);
                    notification.AddSeparator();
                    notification.AddOption("Apagar", Shutdown_Click);

                    Current.Dispatcher.Invoke(() =>
                    {
                        notification.Show("Ejecutando");
                        log.Info("Aplicación iniciada.");
                    });
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                    log.Info(ex.GetBaseException().Message);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
            }).Show();
        }

        private void Configuration_Click(object sender, EventArgs e)
        {
            if (WindowExist<ConfigurationWindow>())
            {
                return;
            }

            _ = new ConfigurationWindow().ShowDialog();
        }

        private void PaymentMethodConfig_Click(object sender, EventArgs e)
        {
            if (WindowExist<PaymentMethodConfigWindow>())
            {
                return;
            }

            _ = new PaymentMethodConfigWindow().ShowDialog();
        }

        private void TicketLog_Click(object sender, EventArgs e)
        {
            if (WindowExist<TicketLogWindow>())
            {
                return;
            }

            new TicketLogWindow().Show();
        }

        private void Shutdown_Click(object sender, EventArgs e)
        {
            Current.Shutdown();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            notification?.Dispose();
            log.Info("Aplicación apagada.");
        }

        public static bool WindowExist<T>()
        {
            return Current.Windows.OfType<T>().Any();
        }
    }
}
