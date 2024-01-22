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
    public class TInvoice
    {
        public int id { get; set; }
        public int idClient { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Closed { get; set; }
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

        public static List<TInvoice> GetAll(bool withClosedOnes = true)
        {
            var db = new RegistosRetroDB();
            var result = new List<TInvoice>();
            var dbResult = new List<Invoices>();
            if (withClosedOnes)
                dbResult = db.Invoices.Where(x => x.Active).ToList();
            else
                dbResult = db.Invoices.Where(x => x.Active && x.Closed).ToList();
            
            foreach (var item in dbResult)
                result.Add(ConvertDatabaseObject(item));
            return result;
        }

        public static List<TInvoice> GetAllFromClient(int idClient, bool withClosedOnes = true)
        {
            var db = new RegistosRetroDB();
            var result = new List<TInvoice>();
            var dbResult = new List<Invoices>();
            if (withClosedOnes)
                dbResult = db.Invoices.Where(x => x.Active && x.Client == idClient).ToList();
            else
                dbResult = db.Invoices.Where(x => x.Active && x.Closed && x.Client == idClient).ToList();

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

        public static TInvoice Update(int id, decimal totalAmount)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Invoices.Where(x => x.id == id).Single();
            dbResult.TotalAmount = totalAmount;
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
    }
}
