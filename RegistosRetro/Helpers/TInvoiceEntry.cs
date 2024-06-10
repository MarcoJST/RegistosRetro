using System;
using System.Collections.Generic;
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

        public int id { get; set; }
        public int idInvoice { get; set; }
        public int idClient { get; set; }
        public string ClientName { get; set; }
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
            get { return Math.Round(_totalAmount, 2); }
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

        public static List<TInvoiceEntry> InitializeList(List<Business.TInvoiceEntry> invoiceEntries)
        {
            var result = new List<TInvoiceEntry>();
            foreach (var item in invoiceEntries)
                result.Add(Initialize(item));
            return result;
        }

        public static TInvoiceEntry Initialize(Business.TInvoiceEntry invoiceEntry)
        {
            var invoice = Business.TInvoice.Get(invoiceEntry.idInvoice);
            var service = Business.TService.Get(invoiceEntry.idService);
            var client = invoice.Client;
            var result = new TInvoiceEntry();
            result.id = invoiceEntry.id;
            result.Description = invoiceEntry.Description;
            result.Amount = invoiceEntry.Amount;
            result.Local = invoiceEntry.Local;
            result.Quantity = invoiceEntry.Quantity;
            result.TotalAmount = invoiceEntry.TotalAmount;
            result.ServiceDate = invoiceEntry.ServiceDate;
            result.ServiceReference = service.Reference;
            result.ServiceName = service.Service;
            result.idInvoice = invoiceEntry.idInvoice;
            result.idClient = invoice.idClient;
            result.ClientName = client.Name;
            return result;
        }

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
