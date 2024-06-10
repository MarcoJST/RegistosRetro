using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RegistosRetro.Helpers
{
    public class TInvoice : INotifyPropertyChanged
    {
        private int _id;
        private int _idClient;
        private string _clientName;
        private decimal _totalAmount;
        private bool _closed;
        private DateTime _creationDate;
        private decimal _paid;
        private decimal _notPaid;

        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public int idClient
        {
            get => _idClient;
            set
            {
                _idClient = value;
                OnPropertyChanged();
            }
        }
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged();
            }
        }
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged();
            }
        }
        public bool Closed
        {
            get => _closed;
            set
            {
                _closed = value;
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
        public decimal Paid
        {
            get => _paid;
            set
            {
                _paid = value;
                OnPropertyChanged();
            }
        }
        public decimal NotPaid
        {
            get => _notPaid;
            set
            {
                _notPaid = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<TInvoice> InitializeList(List<Business.TInvoice> invoices)
        {
            var result = new List<TInvoice>();
            foreach (var item in invoices)
                result.Add(Initialize(item));
            return result;
        }

        public static TInvoice Initialize(Business.TInvoice invoice)
        {
            var result = new TInvoice();
            result.id = invoice.id;
            result.idClient = invoice.idClient;
            result.ClientName = invoice.Client.Name;
            result.TotalAmount = invoice.TotalAmount;
            result.Closed = invoice.Closed;
            result.CreationDate = invoice.CreationDate;
            result.TotalAmount = invoice.TotalAmount;
            result.Paid = invoice.Paid;
            result.NotPaid = invoice.NotPaid;
            return result;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}