using Database.RegistosRetro;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var result = new TInvoicePayment();
            result.id = dbObject.id;
            result.idInvoice = dbObject.Invoice;
            result.Amount = dbObject.Amount;
            result.Observations = dbObject.Observations;
            result.PaymentDate = dbObject.PaymentDate;
            result.CreationDate = dbObject.CreationDate;
            return result;
        }

        public static TInvoicePayment Get(int id)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.InvoicePayments.Where(x => x.id == id).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static List<TInvoicePayment> GetAllFromInvoice(int idInvoice)
        {
            var db = new RegistosRetroDB();
            var result = new List<TInvoicePayment>();
            var dbResult = db.InvoicePayments.Where(x => x.Invoice == idInvoice).ToList();

            foreach (var item in dbResult)
                result.Add(ConvertDatabaseObject(item));
            return result;
        }

        public static TInvoicePayment Add(int idInvoice, decimal amount, string observations, DateTime paymentDate)
        {
            var db = new RegistosRetroDB();
            var dbResult = new InvoicePayments();
            dbResult.Invoice = idInvoice;
            dbResult.Observations = string.IsNullOrEmpty(observations) ? null : observations;
            dbResult.Amount = amount;
            dbResult.CreationDate = DateTime.Now;
            dbResult.PaymentDate = paymentDate;
            db.InvoicePayments.Add(dbResult);
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static TInvoicePayment Update(int id, decimal amount, string observations, DateTime paymentDate)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.InvoicePayments.Where(x => x.id == id).Single();
            dbResult.Observations = string.IsNullOrEmpty(observations) ? null : observations;
            dbResult.Amount = amount;
            dbResult.CreationDate = DateTime.Now;
            dbResult.PaymentDate = paymentDate;
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static void Delete(int id)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.InvoicePayments.Where(x => x.id == id).Single();
            db.InvoicePayments.Remove(dbResult);
            db.SaveChanges();
        }

    }
}
