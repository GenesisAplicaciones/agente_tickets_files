using ACTools.NetFramework.DataBinding;
using AgenteTickets.AppDataBase.DAO;
using AgenteTickets.AppDataBase.Models;
using AgenteTickets.Persistence;
using AgenteTickets.Views;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace AgenteTickets.ViewModels
{
    public class PaymentMethodConfigVM : ViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PaymentMethodConfigVM()
        {
            CloseCommand = new RelayCommand<Window>(Close);
            InitializeCommand = new RelayCommand<Window>(Initialize);
            SaveCommand = new RelayCommand<Window>(Save);
            DeleteCommand = new RelayCommand<object[]>(x => Delete((Window)x[0], (int)x[1]));
            ClearCommand = new RelayCommand(Clear);
            PaymentMethodsConfig = new List<PaymentMethodConfig>();
            Clear();
        }

        private void Initialize(Window window)
        {
            new AppLoaderWindow(loading =>
            {
                loading.ModalText = "Recuperando información...";

                Enable(window, false);

                try
                {
                    Get();
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

        private void Delete(Window window, int id)
        {
            bool confirm = MessageBox.Show("¿Seguro que deseas eliminar la forma de pago?", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

            if (!confirm)
            {
                return;
            }

            new AppLoaderWindow(loading =>
            {
                loading.ModalText = "Eliminando...";

                Enable(window, false);

                try
                {
                    _ = PaymentMethodConfigDAO.Delete(id);
                    Get();
                    AppInstances.PaymentMethodsConfig = PaymentMethodsConfig;
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

        private void Save(Window window)
        {
            new AppLoaderWindow(loading =>
            {
                loading.ModalText = "Guardando...";

                Enable(window, false);

                try
                {
                    _ = PaymentMethodConfigDAO.Create(PaymentMethodConfig);
                    Clear();
                    Get();
                    AppInstances.PaymentMethodsConfig = PaymentMethodsConfig;
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

        private void Clear()
        {
            PaymentMethodConfig = new PaymentMethodConfig();
        }

        private void Get()
        {
            PaymentMethodsConfig = PaymentMethodConfigDAO.Get();
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            Window window = sender as Window;

            if (PaymentMethodsConfig.Any())
            {
                window.DialogResult = true;
            }
            else
            {
                bool cancelEvent = MessageBox.Show("La configuración es necesaria. El ticket se enviará sin forma de pago o en su caso con la existentes en la configuración. ¿Estás seguro?", "Cerrar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No;

                e.Cancel = cancelEvent;
            }
        }

        private PaymentMethodConfig paymentMethodConfig;

        public PaymentMethodConfig PaymentMethodConfig
        {
            get => paymentMethodConfig;
            set
            {
                paymentMethodConfig = value;
                OnPropertyChanged();
            }
        }


        private List<PaymentMethodConfig> paymentMethodsConfig;
        public List<PaymentMethodConfig> PaymentMethodsConfig
        {
            get => paymentMethodsConfig;
            set
            {
                paymentMethodsConfig = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<Window> CloseCommand { get; private set; }
        public RelayCommand<Window> InitializeCommand { get; private set; }
        public RelayCommand<Window> SaveCommand { get; private set; }
        public RelayCommand<object[]> DeleteCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
    }
}
