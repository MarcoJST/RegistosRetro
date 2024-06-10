using Database.RegistosRetro;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business
{
    public class TInvoice
    {
        public int id { get; set; }
        public int idClient { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Closed { get; set; }
        public DateTime CreationDate { get; set; }
        public TClient Client { get { return TClient.Get(idClient); } }
        public decimal Paid { get { return GetPaidValue(id); } }
        public decimal NotPaid { get { return GetUnpaidValue(id); } }

        public TInvoice()
        {
            id = -1;
            idClient = -1;
            TotalAmount = 0.00m;
            CreationDate = new DateTime();
        }

        private static TInvoice ConvertDatabaseObject(Invoices dbObject)
        {
            var result = new TInvoice();
            result.id = dbObject.id;
            result.idClient = dbObject.Client;
            result.TotalAmount = dbObject.TotalAmount;
            result.CreationDate = dbObject.CreationDate;
            return result;
        }

        public static TInvoice Get(int id)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Invoices.Where(x => x.id == id).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static List<TInvoice> GetAll()
        {
            return DynamicSearch();
        }

        public static List<TInvoice> DynamicSearch(string searchText = "")
        {
            using (var db = new RegistosRetroDB())
            {
                IQueryable<Invoices> query = db.Invoices.Where(x => x.Active);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.Trim();
                    if (decimal.TryParse(searchText, out decimal amount))
                        query = query.Where(x => x.TotalAmount == amount);
                    else
                    {
                        string searchLower = searchText.ToLower();
                        query = query.Include(x=> x.Clients).Where(x => x.Clients.Name.ToLower().Contains(searchLower));
                    }
                }

                var dbResult = query.ToList();
                return dbResult.Select(ConvertDatabaseObject).ToList();
            }
        }

        public static List<TInvoice> GetAllFromClient(int idClient)
        {
            var db = new RegistosRetroDB();
            var result = new List<TInvoice>();
            var dbResult = new List<Invoices>();
            dbResult = db.Invoices.Where(x => x.Active && x.Client == idClient).ToList();

            foreach (var item in dbResult)
                result.Add(ConvertDatabaseObject(item));
            return result;
        }

        public static TInvoice Add(int idClient)
        {
            var db = new RegistosRetroDB();
            var dbResult = new Invoices();
            dbResult.Active = true;
            dbResult.Closed = false;
            dbResult.Client = idClient;
            dbResult.TotalAmount = 0.00m;
            dbResult.CreationDate = DateTime.Now;
            db.Invoices.Add(dbResult);
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static void Delete(int id)
        {
            var db = new RegistosRetroDB();
            var client = db.Invoices.Where(x => x.id == id).Single();
            client.Active = false;
            db.SaveChanges();
        }
    
        public static decimal GetPaidValue(int idInvoice)
        {
            var db = new RegistosRetroDB();
            if (!db.InvoicePayments.Where(x => x.Invoice == idInvoice).Any())
                return 0.00m;
            else
                return db.InvoicePayments.Where(x=> x.Invoice == idInvoice).Sum(x=> x.Amount);
        }

        public static decimal GetUnpaidValue(int idInvoice)
        {
            var invoice = Get(idInvoice);
            return invoice.TotalAmount - GetPaidValue(idInvoice);
        }

        public static TInvoice UpdateTotalAmount(int id)
        {
            var db = new RegistosRetroDB();
            var invoice = db.Invoices.Where(x=> x.id == id).Single();
            invoice.TotalAmount = db.InvoicesEntries.Where(x=> x.Invoice == id).Sum(x=> x.TotalAmount);
            db.SaveChanges();
            return ConvertDatabaseObject(invoice);
        }

        public static TInvoice UpdateClient(int idInvoice, int idClient)
        {
            var db = new RegistosRetroDB();
            var invoice = db.Invoices.Where(x => x.id == idInvoice).Single();
            invoice.Client = idClient;
            db.SaveChanges();
            return ConvertDatabaseObject(invoice);
        }
    }
}
