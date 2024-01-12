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
    public class TInvoiceEntry
    {
        public int id { get; set; }
        public int idInvoice { get; set; }
        public int idService { get; set; }
        public string Description { get; set; }
        public string Local { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalAmount { get { return Amount * Quantity; } }
        public DateTime ServiceDate { get; set; }
        public DateTime CreationDate { get; set; }

        public TInvoiceEntry()
        {
            id = -1;
            idInvoice = 0;
            idService = 0;
            Description = "";
            Local = "";
            Amount = 0.00m;
            Quantity = 0.00m;
            ServiceDate = new DateTime();
            CreationDate = new DateTime();
        }

        private static TInvoiceEntry ConvertDatabaseObject(InvoicesEntries dbObject)
        {
            return new TInvoiceEntry();
        }

        public static TInvoiceEntry Get(int id)
        {
            return new TInvoiceEntry();
        }

        public static List<TInvoiceEntry> GetAllFromInvoice(int idInvoice)
        {
            return new List<TInvoiceEntry>();
        }

        public static TInvoiceEntry Add(int idInvoice, int idService, string description, string local, 
            decimal amount, decimal quantity, DateTime serviceDate)
        {
            return new TInvoiceEntry();
        }

        public static TInvoiceEntry Update(int id, int idService, string description, string local,
            decimal amount, decimal quantity, DateTime serviceDate)
        {
            return new TInvoiceEntry();
        }

        public static void Delete(int id)
        {
        }
    }
}
