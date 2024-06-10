using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RegistosRetro.Helpers
{
    public class TInvoicePayment : INotifyPropertyChanged
    {
        private int _id;
        private int _idInvoice;
        private decimal _amount;
        private string _observations;
        private DateTime _paymentDate;
        private DateTime _creationDate;

        public int id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("id");
                }
            }
        }
        public int idInvoice
        {
            get { return _idInvoice; }
            set
            {
                if (_idInvoice != value)
                {
                    _idInvoice = value;
                    OnPropertyChanged("idInvoice");
                }
            }
        }
        public decimal Amount
        {
            get { return Math.Round(_amount, 2); }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }
        public string Observations
        {
            get { return _observations; }
            set
            {
                if (_observations != value)
                {
                    _observations = value;
                    OnPropertyChanged("Observations");
                }
            }
        }
        public DateTime PaymentDate
        {
            get { return _paymentDate; }
            set
            {
                if (_paymentDate != value)
                {
                    _paymentDate = value;
                    OnPropertyChanged("PaymentDate");
                }
            }
        }
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                if (_creationDate != value)
                {
                    _creationDate = value;
                    OnPropertyChanged("CreationDate");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<TInvoicePayment> InitializeList(List<Business.TInvoicePayment> invoicePayments)
        {
            var result = new List<TInvoicePayment>();
            foreach (var item in invoicePayments)
                result.Add(Initialize(item));
            return result;
        }

        public static TInvoicePayment Initialize(Business.TInvoicePayment invoicePayment)
        {
            var result = new TInvoicePayment();
            result.id = invoicePayment.id;
            result.idInvoice = invoicePayment.idInvoice;
            result.Observations = invoicePayment.Observations;
            result.Amount = invoicePayment.Amount;
            result.PaymentDate = invoicePayment.PaymentDate;
            result.CreationDate = invoicePayment.CreationDate;
            return result;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
