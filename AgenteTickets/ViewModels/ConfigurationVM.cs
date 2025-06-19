using ACTools.NetFramework.DataBinding;
using ACTools.NetFramework.Models;
using AgenteTickets.APISelfbilling;
using AgenteTickets.APISelfbilling.Services;
using AgenteTickets.AppDataBase;
using AgenteTickets.AppDataBase.DAO;
using AgenteTickets.AppDataBase.Models;
using AgenteTickets.ClientData.Enums;
using AgenteTickets.Enums;
using AgenteTickets.Models;
using AgenteTickets.Persistence;
using AgenteTickets.Utils;
using AgenteTickets.Views;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace AgenteTickets.ViewModels
{
    public class ConfigurationVM : ViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ConfigurationVM()
        {
            InitializeCommand = new RelayCommand<Window>(Initialize);
            SaveCommand = new RelayCommand<Window>(Save);
            CloseCommand = new RelayCommand<Window>(Close);
            FindFilePathCommand = new RelayCommand(FindFilePath);
            FindResponsePathCommand = new RelayCommand(FindResponsePath);

            FileTypes = new List<SelectOption<FileType>>
            {
                new SelectOption<FileType>
                {
                    Text = "Aloha XML",
                    Value = FileType.AlohaXML
                }
            };

            FileReaderConfig = new FileReaderConfig
            {
                Id = 1,
            };

            SelfbillingConfig = new SelfbillingConfig
            {
                Id = 1,
                LoadIntervalTickets = new TimeSpan(0, 5, 0),
                TimeLoadCancellations = new TimeSpan(23, 59, 59)
            };
        }

        private void Initialize(Window window)
        {
            new AppLoaderWindow(loading =>
            {
                loading.ModalText = "Recuperando información...";

                Enable(window, false);

                try
                {
                    IsEditing = AppDBInstance.DBFileExist;

                    if (IsEditing)
                    {
                        FileReaderConfig = FileReaderConfigDAO.Find();
                        SelfbillingConfig = SelfbillingConfigDAO.Find();
                    }

                    window.Dispatcher.Invoke(() =>
                    {
                        (window.FindName("APIPassword") as PasswordBox).Password = SelfbillingConfig.ApiPassword;
                    });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    loading.Dispatcher.Invoke(() => loading.Hide());
                    _ = MessageBox.Show(ex.GetBaseException().Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Enable(window);
                }

            })
            {
                Owner = window
            }.Show();
        }

        private (string, string) ValidateFieldsDataBase()
        {
            if (FileReaderConfig.FileType == FileType.NONE)
            {
                return ("Tipo de archivo", "Campo requerido.");
            }

            if (string.IsNullOrWhiteSpace(FileReaderConfig.Path))
            {
                return ("Ubicación archivos", "Campo requerido.");
            }
            else if (!Directory.Exists(FileReaderConfig.Path))
            {
                return ("Ubicación archivos", "No existe.");
            }

            if (string.IsNullOrWhiteSpace(FileReaderConfig.ResponsePath))
            {
                return ("Ubicación respuestas", "Campo requerido.");
            }
            else if (!Directory.Exists(FileReaderConfig.ResponsePath))
            {
                return ("Ubicación respuestas", "No existe.");
            }

            return (null, null);
        }

        private (string, string) ValidateFieldsApi(bool test = false)
        {
            if (string.IsNullOrWhiteSpace(SelfbillingConfig.ApiUrl))
            {
                return ("URL API", "Campo requerido.");
            }

            if (string.IsNullOrWhiteSpace(SelfbillingConfig.ApiUser))
            {
                return ("Usuario API", "Campo requerido.");
            }

            if (string.IsNullOrWhiteSpace(SelfbillingConfig.ApiPassword))
            {
                return ("Contraseña API", "Campo requerido.");
            }

            if (test)
            {
                return (null, null);
            }

            return (null, null);
        }

        private bool ValidateForm(ConfigFormSection section = ConfigFormSection.ALL, bool test = false)
        {
            string message;
            string caption;

            switch (section)
            {
                case ConfigFormSection.DATABASE:
                    (caption, message) = ValidateFieldsDataBase();
                    break;
                case ConfigFormSection.API:
                    (caption, message) = ValidateFieldsApi(test);
                    break;
                case ConfigFormSection.ALL:
                    (caption, message) = ValidateFieldsDataBase();

                    if (string.IsNullOrEmpty(message))
                    {
                        (caption, message) = ValidateFieldsApi();
                    }
                    break;
                default:
                    message = null;
                    caption = null;
                    break;
            }

            bool isValid = string.IsNullOrEmpty(message);

            if (!isValid)
            {
                _ = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Stop);
            }

            return isValid;
        }

        private void GenerateToken()
        {
            SelfbillingHttp.ResfreshBaseUrl(SelfbillingConfig.ApiUrl);
            SelfbillingResponse response = AuthService.GenerateToken(SelfbillingConfig.ApiUser, SelfbillingConfig.ApiPassword);

            if (response.IsSuccessful)
            {
                SelfbillingConfig.ApiToken = response.Token;
                SelfbillingConfig.ApiTokenExp = response.DecodedToken.Exp;
            }
            else
            {
                throw new Exception(response.Message);
            }
        }

        private void Save(Window window)
        {
            if (!ValidateForm())
            {
                return;
            }

            new AppLoaderWindow(loading =>
            {
                try
                {
                    loading.ModalText = "Validando información...";

                    Enable(window, false);


                    if (!IsEditing || Compare.ObjectChanges(AppInstances.SelfbillingConfig, SelfbillingConfig, "ApiUrl", "ApiUser", "ApiPassword"))
                    {
                        GenerateToken();
                    }


                    loading.ModalText = "Guardando...";

                    bool resetCancellationsJob = IsEditing && Compare.ObjectChanges(AppInstances.SelfbillingConfig, SelfbillingConfig, nameof(SelfbillingConfig.TimeLoadCancellationsSeconds));
                    bool resetTickesJob = IsEditing && Compare.ObjectChanges(AppInstances.SelfbillingConfig, SelfbillingConfig, nameof(SelfbillingConfig.LoadIntervalTicketsSeconds));

                    bool saved = IsEditing
                        ? FileReaderConfigDAO.Update(FileReaderConfig) && SelfbillingConfigDAO.Update(SelfbillingConfig)
                        : FileReaderConfigDAO.Create(FileReaderConfig) && SelfbillingConfigDAO.Create(SelfbillingConfig);

                    log.Info("Configuración guardada.");

                    if (AppDBInstance.CreateBackupDBFile())
                    {
                        log.Info("Copia de seguridad creada.");
                    }
                    else
                    {
                        log.Info("No se pudo crear la copia de seguridad.");
                    }

                    if (saved)
                    {
                        AppInstances.FileReaderConfig = FileReaderConfig;
                        AppInstances.SelfbillingConfig = SelfbillingConfig;

                        if (resetTickesJob)
                        {
                            while (AppInstances.UploadTicketsJob.RunningTask)
                            {
                                Thread.Sleep(1000);
                            }

                            AppInstances.UploadTicketsJob.Change(SelfbillingConfig.LoadIntervalTickets, SelfbillingConfig.LoadIntervalTickets, true);
                        }

                        //if (resetCancellationsJob)
                        //{
                        //    while (AppInstances.UploadCancellationsJob.RunningTask)
                        //    {
                        //        Thread.Sleep(1000);
                        //    }

                        //    AppInstances.UploadCancellationsJob.Change(SelfbillingConfig.TimeLoadCancellations, true);
                        //}

                        log.Info("Jobs reiniciados.");
                    }

                    Close(window, saved);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    loading.Dispatcher.Invoke(() => loading.Hide());
                    _ = MessageBox.Show(ex.GetBaseException().Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Enable(window);
                }
            }).Show();
        }

        public void APIPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            string apiPassword = (sender as PasswordBox).Password;

            if (string.IsNullOrWhiteSpace(apiPassword))
            {
                SelfbillingConfig.ApiPassword = null;
            }
            else
            {
                SelfbillingConfig.ApiPassword = Security.GetMD5Hash(apiPassword);
            }
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            Window window = sender as Window;

            if (window.DialogResult ?? false)
            {
                return;
            }

            bool cancelEvent = !IsEditing && MessageBox.Show("La configuración es necesaria. Si cierras la ventana, la aplicación se detendrá. ¿Estás seguro?", "Configuración", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No;

            if (!cancelEvent)
            {
                if (!IsEditing)
                {
                    AppDBInstance.DeleteDBFile();
                    log.Info("Cofiguración inicial omitida.");
                }
            }

            e.Cancel = cancelEvent;
        }

        private void FindFilePath()
        {
            using (Forms.FolderBrowserDialog dialog = new Forms.FolderBrowserDialog())
            {
                dialog.Description = "Selecciona una carpeta";

                if (dialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    FileReaderConfig.Path = dialog.SelectedPath;
                }
            }
        }

        private void FindResponsePath()
        {
            using (Forms.FolderBrowserDialog dialog = new Forms.FolderBrowserDialog())
            {
                dialog.Description = "Selecciona una carpeta";

                if (dialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    FileReaderConfig.ResponsePath = dialog.SelectedPath;
                }
            }
        }

        public bool IsEditing { get; private set; }

        private FileReaderConfig fileReaderConfig;
        public FileReaderConfig FileReaderConfig
        {
            get => fileReaderConfig;
            set
            {
                fileReaderConfig = value;
                OnPropertyChanged();
            }
        }

        private SelfbillingConfig selfbillingConfig;
        public SelfbillingConfig SelfbillingConfig
        {
            get => selfbillingConfig;
            set
            {
                selfbillingConfig = value;
                OnPropertyChanged();
            }
        }

        private List<SelectOption<FileType>> fileTypes;
        public List<SelectOption<FileType>> FileTypes
        {
            get => fileTypes;
            set
            {
                fileTypes = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<Window> InitializeCommand { get; private set; }
        public RelayCommand<Window> SaveCommand { get; private set; }
        public RelayCommand<Window> CloseCommand { get; private set; }
        public RelayCommand FindFilePathCommand { get; private set; }
        public RelayCommand FindResponsePathCommand { get; private set; }
    }
}
