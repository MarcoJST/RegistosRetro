using Database.RegistosRetro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class TClient
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal NotPaid { get; set; }
        public DateTime CreationDate { get; set; }

        public TClient()
        {
            id = -1;
            Name = "";
            Address = "";
            Phone = "";
            Email = "";
            Invoiced = 0;
            Paid = 0;
            NotPaid = 0;
            CreationDate = new DateTime();
        }

        private static TClient ConvertDatabaseObject(Clients dbObject)
        {
            var invoices = TInvoice.GetAllFromClient(dbObject.id);
            var result = new TClient();
            result.id = dbObject.id;
            result.Name = dbObject.Name;
            result.Address = dbObject.Address;
            result.Phone = dbObject.Phone;
            result.Email = dbObject.Email;
            result.CreationDate = dbObject.CreationDate;
            result.Invoiced = invoices.Sum(x => x.TotalAmount);
            result.Paid = invoices.Sum(x => x.Paid);
            result.NotPaid = invoices.Sum(x => x.NotPaid);
            return result;
        }

        public static TClient Get(int id)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Clients.Where(x => x.id == id).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static TClient Get(string name)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Clients.Where(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.Active).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static TClient GetByPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone.Trim()))
                throw new Exception("O argumento \"phone\" não é uma string válida");

            if (!ExistsByPhone(phone))
                throw new Exception("Não existe nenhum cliente com o número de telefone facultado");

            var db = new RegistosRetroDB();
            var dbResult = db.Clients.Where(x => x.Phone.Trim().ToLower() == phone.Trim().ToLower() && x.Active).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static TClient GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email.Trim()))
                throw new Exception("O argumento \"email\" não é uma string válida");

            if (!ExistsByEmail(email))
                throw new Exception("Não existe nenhum cliente com o email facultado");

            var db = new RegistosRetroDB();
            var dbResult = db.Clients.Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Active).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static List<TClient> GetAll()
        {
            return DynamicSearch();
        }

        public static List<TClient> DynamicSearch(string textToSearch = "")
        {
            var db = new RegistosRetroDB();
            var result = new List<TClient>();

            IQueryable<Clients> query = db.Clients.Where(x => x.Active);

            if (!string.IsNullOrEmpty(textToSearch))
            {
                string searchLower = textToSearch.ToLower().Trim();
                query = query.Where(x =>
                    x.Address.ToLower().Contains(searchLower) ||
                    x.Name.ToLower().Contains(searchLower) ||
                    x.Email.ToLower().Contains(searchLower) ||
                    x.Phone.ToLower().Contains(searchLower));
            }

            var dbResult = query.ToList();

            foreach (var item in dbResult)
                result.Add(ConvertDatabaseObject(item));

            return result;
        }


        public static TClient Add(string name, string address, string phone, string email)
        {
            if (Exists(name))
                return Get(name);
            var db = new RegistosRetroDB();
            var dbResult = new Clients();
            dbResult.Name = name.Trim();
            dbResult.Address = string.IsNullOrEmpty(address) ? null : address.Trim();
            dbResult.Phone = string.IsNullOrEmpty(phone) ? null : phone.Trim();
            dbResult.Email = string.IsNullOrEmpty(email) ? null : email.Trim();
            dbResult.CreationDate = DateTime.Now;
            dbResult.Active = true;
            db.Clients.Add(dbResult);
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static TClient Update(int id, string name, string address, string phone, string email)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Clients.Where(x => x.id == id).Single();
            if (dbResult.Name.Trim().ToLower() != name.Trim().ToLower())
                if (Exists(name))
                    throw new Exception("There is already a user with the name \"" + name.Trim() + "\".");

            dbResult.Name = name.Trim();
            dbResult.Address = string.IsNullOrEmpty(address) ? null : address.Trim();
            dbResult.Phone = string.IsNullOrEmpty(phone) ? null : phone.Trim();
            dbResult.Email = string.IsNullOrEmpty(email) ? null : email.Trim();
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static bool Exists(string name, int id = 0)
        {
            var db = new RegistosRetroDB();
            if (id <= 0)
                return db.Clients.Where(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.Active).Any();
            else
                return db.Clients.Where(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.Active && x.id != id).Any();
        }

        public static bool ExistsByEmail(string email, int id = 0)
        {
            if (string.IsNullOrEmpty(email.Trim()))
                return false;

            var db = new RegistosRetroDB();
            if (id <= 0)
                return db.Clients.Where(x => x.Email.ToLower().Trim() == email.ToLower().Trim() && x.Active).Any();
            else
                return db.Clients.Where(x => x.Email.ToLower().Trim() == email.ToLower().Trim() && x.Active && x.id != id).Any();
        }

        public static bool ExistsByPhone(string phone, int id = 0)
        {
            if (string.IsNullOrEmpty(phone.Trim()))
                return false;

            var db = new RegistosRetroDB();
            if (id <= 0)
                return db.Clients.Where(x => x.Phone.ToLower().Trim() == phone.ToLower().Trim() && x.Active).Any();
            else
                return db.Clients.Where(x => x.Phone.ToLower().Trim() == phone.ToLower().Trim() && x.Active && x.id != id).Any();
        }

        public static void Delete(int id)
        {
            var db = new RegistosRetroDB();
            var client = db.Clients.Where(x => x.id == id).Single();
            client.Active = false;
            db.SaveChanges();
        }

        public static decimal GetValueNotPaid(int id)
        {
            var db = new RegistosRetroDB();
            decimal total = 0;
            decimal paid = 0;
            if (db.InvoicesEntries.Where(x => x.Invoices.Client == id).Any())
                total = db.InvoicesEntries.Where(x => x.Invoices.Client == id && x.Invoices.Active).Select(x => x.TotalAmount).Sum();

            if (db.InvoicePayments.Where(x => x.Invoices.Client == id).Any())
                paid = db.InvoicePayments.Where(x => x.Invoices.Client == id && x.Invoices.Active).Select(x => x.Amount).Sum();

            return total - paid;
        }
    }
}
