using ACTools.NetFramework.Models;
using AgenteTickets.Utils;
using System;

namespace AgenteTickets.AppDataBase.Models
{
    public class SelfbillingConfig : Model
    {
        public virtual int Id { get; set; }

        private string apiUrl;
        public virtual string ApiUrl
        {
            get => apiUrl;
            set
            {
                apiUrl = value;
                OnPropertyChanged();
            }
        }

        private string apiUser;
        public virtual string ApiUser {
            get => apiUser;
            set
            {
                apiUser = value;
                OnPropertyChanged();
            }
        }

        private string apiPassword;
        public virtual string ApiPassword {
            get => apiPassword;
            set
            {
                apiPassword = value;
                OnPropertyChanged();
            }
        }

        private string serie;
        public virtual string Serie
        {
            get => serie;
            set
            {
                serie = value;
                OnPropertyChanged();
            }
        }

        public virtual long LoadIntervalTicketsSeconds { get; set; }
        public virtual TimeSpan LoadIntervalTickets
        {
            get => TimeSpan.FromSeconds(LoadIntervalTicketsSeconds);
            set
            {
                LoadIntervalTicketsSeconds = (long)value.TotalSeconds;
                OnPropertyChanged();
            }
        }

        public virtual long TimeLoadCancellationsSeconds { get; set; }
        public virtual TimeSpan TimeLoadCancellations
        {
            get => TimeSpan.FromSeconds(TimeLoadCancellationsSeconds);
            set
            {
                TimeLoadCancellationsSeconds = (long)value.TotalSeconds;
                OnPropertyChanged();
            }
        }

        private bool loadPreviousDayCancellations;
        public virtual bool LoadPreviousDayCancellations
        {
            get => loadPreviousDayCancellations;
            set
            {
                loadPreviousDayCancellations = value;
                OnPropertyChanged();
            }
        }


        public virtual string ApiToken { get; set; }
        public virtual long? ApiTokenExp { get; set; }

        public virtual bool TokenIsExpired => !ApiTokenExp.HasValue || (ApiTokenExp - JWT.UnixTimestamp <= 300);
    }
}
