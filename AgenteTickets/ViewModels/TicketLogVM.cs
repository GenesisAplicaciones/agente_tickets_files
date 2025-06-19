using ACTools.NetFramework.DataBinding;
using ACTools.NetFramework.Models;
using AgenteTickets.AppDataBase.DAO;
using AgenteTickets.AppDataBase.Enums;
using AgenteTickets.AppDataBase.Models;
using AgenteTickets.Views;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace AgenteTickets.ViewModels
{
    public class TicketLogVM : ViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TicketLogVM()
        {
            InitializeCommand = new RelayCommand<Window>(Initialize);
            SearchCommand = new RelayCommand<Window>(Search);
            CloseCommand = new RelayCommand<Window>(Close);

            Series = new List<SelectOption<string>>
            {
                new SelectOption<string>
                {
                    Text = "Todas",
                    Value = "",
                }
            };

            StatusCodes = new List<SelectOption<string>>
            {
                new SelectOption<string>
                {
                    Text = "Todos",
                    Value = "",
                },
            };

            Methods = new List<SelectOption<TypeMethod>>
            {
                new SelectOption<TypeMethod>
                {
                    Text = "Ambos",
                    Value = TypeMethod.NONE
                },
                new SelectOption<TypeMethod>
                {
                    Text = "Automático",
                    Value = TypeMethod.AUTOMATIC
                },
                new SelectOption<TypeMethod>
                {
                    Text = "Manual",
                    Value = TypeMethod.MANUAL
                },
            };

            Actions = new List<SelectOption<TypeAction>>
            {
                new SelectOption<TypeAction>
                {
                    Text = "Ambos",
                    Value = TypeAction.NONE,
                },
                new SelectOption<TypeAction>
                {
                    Text = "Carga",
                    Value = TypeAction.LOAD,
                },
                new SelectOption<TypeAction>
                {
                    Text = "Cacenlación",
                    Value = TypeAction.CANCELLATION,
                },
            };

            StatusActionOptions = new List<SelectOption<StatusAction>>
            {
                new SelectOption<StatusAction>
                {
                    Text = "Ambos",
                    Value = StatusAction.NONE,
                },
                new SelectOption<StatusAction>
                {
                    Text = "Completado",
                    Value = StatusAction.COMPLETED,
                },
                new SelectOption<StatusAction>
                {
                    Text = "No completado",
                    Value = StatusAction.NOT_COMPLETED,
                },
            };

            ApiResponseCodes = new List<SelectOption<int?>>
            {
                new SelectOption<int?>
                {
                    Text = "Todo",
                    Value = -1,
                },
                new SelectOption<int?>
                {
                    Text = "Sin código",
                    Value = null,
                },
                new SelectOption<int?>
                {
                    Text = "200	[Mensaje específico]",
                    Value = 200,
                },
                new SelectOption<int?>
                {
                    Text = "205	Ticket actualizado con éxito",
                    Value = 205,
                },
                new SelectOption<int?>
                {
                    Text = "300	Ticket ya existe",
                    Value = 300,
                },
                new SelectOption<int?>
                {
                    Text = "400	Ocurrió un error al guardar el registro",
                    Value = 400,
                },
                new SelectOption<int?>
                {
                    Text = "401	Acceso no autorizado",
                    Value = 401,
                },
                new SelectOption<int?>
                {
                    Text = "402	[Mensaje específico]",
                    Value = 402,
                },
                new SelectOption<int?>
                {
                    Text = "403	Sucursal solicitada no existe",
                    Value = 403,
                },
                new SelectOption<int?>
                {
                    Text = "404	Acción no encontrada",
                    Value = 404,
                },
                new SelectOption<int?>
                {
                    Text = "406	Credenciales inválidas",
                    Value = 406,
                },
                new SelectOption<int?>
                {
                    Text = "410	Ticket solicitado no existe",
                    Value = 410,
                },
                new SelectOption<int?>
                {
                    Text = "411	Ticket solicitado no se puede actualizar",
                    Value = 411,
                },
                new SelectOption<int?>
                {
                    Text = "600 Formato de archivo incorrecto. [Mensaje específico]",
                    Value = 600,
                },
                new SelectOption<int?>
                {
                    Text = "601 Ticket no cerrado",
                    Value = 601
                }
            };

            SearchData = new TicketLog
            {
                Id = DateTime.Today,
                TicketSerie = "",
                TicketFolio = "",
                TicketDate = null,
                TicketAmount = null,
                TicketStatusCode = "",
                TypeMethod = TypeMethod.AUTOMATIC,
                TypeAction = TypeAction.LOAD,
                StatusAction = StatusAction.NONE,
                ApiResponseCode = -1,
                Message = "",
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
                    Series.AddRange(TicketLogDAO.GetSeries());
                    StatusCodes.AddRange(TicketLogDAO.GetTicketStatusCodes());
                    TicketLogs = TicketLogDAO.List(SearchData);
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

        private void Search(Window window)
        {
            new AppLoaderWindow(loading =>
            {
                loading.ModalText = "Buscando registros...";

                Enable(window, false);

                try
                {
                    TicketLogs = TicketLogDAO.List(SearchData);
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

        private List<SelectOption<string>> series;
        public List<SelectOption<string>> Series
        {
            get => series;
            set
            {
                series = value;
                OnPropertyChanged();
            }
        }

        private List<SelectOption<string>> statusCodes;
        public List<SelectOption<string>> StatusCodes
        {
            get => statusCodes;
            set
            {
                statusCodes = value;
                OnPropertyChanged();
            }
        }

        private List<SelectOption<TypeMethod>> methods;
        public List<SelectOption<TypeMethod>> Methods
        {
            get => methods;
            set
            {
                methods = value;
                OnPropertyChanged();
            }
        }

        private List<SelectOption<TypeAction>> actions;
        public List<SelectOption<TypeAction>> Actions
        {
            get => actions;
            set
            {
                actions = value;
                OnPropertyChanged();
            }
        }

        private List<SelectOption<StatusAction>> statusActionOptions;
        public List<SelectOption<StatusAction>> StatusActionOptions
        {
            get => statusActionOptions;
            set
            {
                statusActionOptions = value;
                OnPropertyChanged();
            }
        }

        private List<SelectOption<int?>> apiResponseCodes;
        public List<SelectOption<int?>> ApiResponseCodes
        {
            get => apiResponseCodes;
            set
            {
                apiResponseCodes = value;
                OnPropertyChanged();
            }
        }

        private TicketLog searchData;
        public TicketLog SearchData
        {
            get => searchData;
            set
            {
                searchData = value;
                OnPropertyChanged();
            }
        }

        private List<TicketLog> ticketLogs;
        public List<TicketLog> TicketLogs
        {
            get => ticketLogs;
            set
            {
                ticketLogs = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<Window> InitializeCommand { get; private set; }
        public RelayCommand<Window> SearchCommand { get; private set; }
        public RelayCommand<Window> CloseCommand { get; private set; }
    }
}
