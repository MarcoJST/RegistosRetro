using System;
using System.ComponentModel;

namespace RegistosRetro.Helpers
{
    public class TInvoiceEntry : INotifyPropertyChanged
    {
        private string _serviceName;
        private string _serviceReference;
        private decimal _amount;
        private decimal _quantity;
        private decimal _totalAmount;

        public string Description { get; set; }
        public string Local { get; set; }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                    UpdateTotalAmount();
                }
            }
        }

        public decimal Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged("Quantity");
                    UpdateTotalAmount();
                }
            }
        }

        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                if (_serviceName != value)
                {
                    _serviceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }

        public string ServiceReference
        {
            get { return _serviceReference; }
            set
            {
                if (_serviceReference != value)
                {
                    _serviceReference = value;
                    OnPropertyChanged("ServiceReference");
                }
            }
        }

        public DateTime ServiceDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateTotalAmount()
        {
            TotalAmount = Amount * Quantity;
            OnPropertyChanged("TotalAmount");
        }
    }
}
