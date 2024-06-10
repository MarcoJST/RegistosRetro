using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RegistosRetro.Helpers
{
    public class TService : INotifyPropertyChanged
    {
        private int _id;
        private string _pageTitle;
        private string _reference;
        private string _service;
        private decimal _amount;
        private decimal _invoiced;
        private List<Helpers.TInvoiceEntry> _invoiceEntries;
        private DateTime _creationDate;

        public int id
        {
            get => _id;
            set
            {
                _id= value;
                OnPropertyChanged();
            }
        }

        public string Reference
        {
            get => _reference;
            set
            {
                _reference = value;
                OnPropertyChanged();
            }
        }

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged();
            }
        }

        public string Service
        {
            get => _service;
            set
            {
                _service = value;
                OnPropertyChanged();
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public decimal Invoiced
        {
            get => _invoiced;
            set
            {
                _invoiced = value;
                OnPropertyChanged();
            }
        }

        public List<Helpers.TInvoiceEntry> InvoiceEntries
        {
            get => _invoiceEntries;
            set
            {
                _invoiceEntries = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static TService Initialize(int idService)
        {
            var result = new TService();
            var bService = Business.TService.Get(idService);
            result.id = bService.id;
            result.Reference = bService.Reference;
            result.Service = bService.Service;
            result.Amount = bService.Amount;
            result.CreationDate = bService.CreationDate;
            result.InvoiceEntries = Helpers.TInvoiceEntry.InitializeList(Business.TInvoiceEntry.GetAllFromService(idService));
            result.Invoiced = result.InvoiceEntries.Sum(x=> x.TotalAmount);
            result.PageTitle = result.Reference + " - " + result.Service;
            return result;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
