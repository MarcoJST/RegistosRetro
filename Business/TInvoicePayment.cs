using Database.RegistosRetro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business
{
    public class TInvoicePayment
    {
        public int id { get; set; }
        public int idInvoice { get; set; }
        public decimal Amount { get; set; }
        public string Observations { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CreationDate { get; set; }

        public TInvoicePayment()
        {
            id = -1;
            idInvoice = 0;
            Amount = 0.00m;
            Observations = "";
            PaymentDate = new DateTime();
            CreationDate = new DateTime();
        }

        private static TInvoicePayment ConvertDatabaseObject(InvoicePayments dbObject)
        {
            return new TInvoicePayment();
        }

        public static TInvoicePayment Get(int id)
        {
            return new TInvoicePayment();
        }

        public static List<TInvoicePayment> GetAllFromInvoice(int idInvoice)
        {
            return new List<TInvoicePayment>();
        }

        public static TInvoicePayment Add(int idInvoice, decimal amount, string observations, DateTime paymentDate)
        {
            return new TInvoicePayment();
        }

        public static TInvoicePayment Update(int id, decimal amount, string observations, DateTime paymentDate)
        {
            return new TInvoicePayment();
        }

        public static void Delete(int id)
        {
        }

    }
}
