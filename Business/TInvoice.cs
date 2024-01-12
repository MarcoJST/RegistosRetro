using Database.RegistosRetro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class TInvoice
    {
        public int id { get; set; }
        public int idClient { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreationDate { get; set; }

        public TInvoice()
        {
            id = -1;
            idClient = -1;
            TotalAmount = 0.00m;
            CreationDate = new DateTime();
        }

        private static TInvoice ConvertDatabaseObject(Invoices dbObject)
        {
            return new TInvoice();
        }

        public static TInvoice Get(int id)
        {
            return new TInvoice();
        }

        public static List<TInvoice> GetAll(bool withClosedOnes = true)
        {
            return new List<TInvoice>();
        }

        public static List<TInvoice> GetAllFromClient(int idClient, bool withClosedOnes = true)
        {
            return new List<TInvoice>();
        }

        public static TInvoice Add(int idClient)
        {
            return new TInvoice();
        }

        public static TInvoice Update(int id, decimal totalAmount)
        {
            return new TInvoice();
        }

        public static void Delete(int id)
        {
        }
    }
}
