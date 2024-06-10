using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RegistosRetro.Helpers
{
    public class TClient : INotifyPropertyChanged
    {
        private int _id;
        private string _pageTitle;
        private string _name;
        private string _address;
        private string _phone;
        private string _email;
        private decimal _invoiced;
        private decimal _paid;
        private decimal _notpaid;
        private List<Helpers.TInvoice> _invoices;
        private DateTime _creationDate;

        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
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

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
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
            get => _notpaid;
            set
            {
                _notpaid = value;
                OnPropertyChanged();
            }
        }

        public List<Helpers.TInvoice> Invoices
        {
            get => _invoices;
            set
            {
                _invoices = value;
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

        public static TClient Initialize(int idClient)
        {
            var result = new TClient();
            var bClient = Business.TClient.Get(idClient);
            result.id = bClient.id;
            result.Name = bClient.Name;
            result.Address = bClient.Address;
            result.Phone = bClient.Phone;
            result.Email = bClient.Email;
            result.Invoices = Helpers.TInvoice.InitializeList(Business.TInvoice.GetAllFromClient(idClient));
            result.Invoiced = result.Invoices.Sum(x => x.TotalAmount);
            result.Paid = result.Invoices.Sum(x => x.Paid);
            result.NotPaid = result.Invoices.Sum(x => x.NotPaid);
            result.CreationDate = bClient.CreationDate;
            result.PageTitle = result.Name;
            return result;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}