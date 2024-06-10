using Database.RegistosRetro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

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
        public decimal TotalAmount { get; set; }
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
            TotalAmount = 0.00m;
            ServiceDate = new DateTime();
            CreationDate = new DateTime();
        }

        private static TInvoiceEntry ConvertDatabaseObject(InvoicesEntries dbObject)
        {
            var result = new TInvoiceEntry();
            result.id = dbObject.id;
            result.idInvoice = dbObject.Invoice;
            result.idService = dbObject.Service;
            result.Description = dbObject.Description;
            result.Local = dbObject.Local;
            result.Amount = dbObject.Amount;
            result.Quantity = dbObject.Quantity;
            result.TotalAmount = dbObject.Quantity * dbObject.Amount;
            result.ServiceDate = dbObject.ServiceDate;
            result.CreationDate = dbObject.CreationDate;
            return result;
        }

        public static TInvoiceEntry Get(int id)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.InvoicesEntries.Where(x => x.id == id).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static List<TInvoiceEntry> GetAllFromInvoice(int idInvoice)
        {
            var db = new RegistosRetroDB();
            var result = new List<TInvoiceEntry>();
            var dbResult = db.InvoicesEntries.Where(x => x.Invoice== idInvoice).ToList();

            foreach (var item in dbResult)
                result.Add(ConvertDatabaseObject(item));
            return result;
        }

        public static List<TInvoiceEntry> GetAllFromService(int idService)
        {
            var db = new RegistosRetroDB();
            var result = new List<TInvoiceEntry>();
            var dbResult = new List<InvoicesEntries>();
            dbResult = db.InvoicesEntries.Where(x => x.Service == idService).ToList();

            foreach (var item in dbResult)
                result.Add(ConvertDatabaseObject(item));
            return result;
        }

        public static TInvoiceEntry Add(int idInvoice, int idService, string description, string local, 
            decimal amount, decimal quantity, DateTime serviceDate)
        {
            var db = new RegistosRetroDB();
            var dbResult = new InvoicesEntries();
            dbResult.Invoice = idInvoice;
            dbResult.Service = idService;
            dbResult.Description = string.IsNullOrEmpty(description) ? null : description;
            dbResult.Local = string.IsNullOrEmpty(local) ? null : local;
            dbResult.Amount = amount;
            dbResult.Quantity = quantity;
            dbResult.TotalAmount = amount * quantity;
            dbResult.CreationDate = DateTime.Now;
            dbResult.ServiceDate = serviceDate;
            db.InvoicesEntries.Add(dbResult);
            db.SaveChanges();
            TInvoice.UpdateTotalAmount(idInvoice);
            return ConvertDatabaseObject(dbResult);
        }

        public static TInvoiceEntry Update(int id, int idService, string description, string local,
            decimal amount, decimal quantity, DateTime serviceDate)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.InvoicesEntries.Where(x=> x.id == id).Single();
            dbResult.Service = idService;
            dbResult.Description = string.IsNullOrEmpty(description) ? null : description;
            dbResult.Local = string.IsNullOrEmpty(local) ? null : local;
            dbResult.Amount = amount;
            dbResult.Quantity = quantity;
            dbResult.TotalAmount = amount * quantity;
            dbResult.CreationDate = DateTime.Now;
            dbResult.ServiceDate = serviceDate;
            db.SaveChanges();
            TInvoice.UpdateTotalAmount(dbResult.Invoice);
            return ConvertDatabaseObject(dbResult);
        }

        public static void Delete(int id)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.InvoicesEntries.Where(x => x.id == id).Single();
            int idInvoice = dbResult.Invoice;
            db.InvoicesEntries.Remove(dbResult);
            db.SaveChanges();
            TInvoice.UpdateTotalAmount(idInvoice);
        }
    }
}
