using ACTools.NetFramework.Models;

namespace AgenteTickets.AppDataBase.Models
{
    public class PaymentMethodConfig : Model
    {
        public virtual int Id { get; set; }

        private string posCode;
        public virtual string POSCode
        {
            get => posCode;
            set
            {
                posCode = value;
                OnPropertyChanged();
            }
        }

        private string description;
        public virtual string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        private string taxCode;

        public virtual string TaxCode
        {
            get => taxCode;
            set
            {
                taxCode = value;
                OnPropertyChanged();
            }
        }

    }
}
