using Database.RegistosRetro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class TService
    {
        public int id { get; set; }
        public string Reference { get; set; }
        public string Service { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreationDate { get; set; }

        public TService()
        {
            id = -1;
            Reference = "";
            Service = "";
            Amount = 0.00m;
            CreationDate = new DateTime();
        }

        private static TService ConvertDatabaseObject(Services dbObject)
        {
            var result = new TService();
            result.id = dbObject.id;
            result.Reference = dbObject.Reference;
            result.Service = dbObject.Service;
            result.Amount = dbObject.Amount;
            result.CreationDate = dbObject.CreationDate;
            return result;
        }

        public static TService Get(int id)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Services.Where(x => x.id == id).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static TService Get(string service)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Services.Where(x => x.Service.Trim().ToLower() == service.Trim().ToLower() && x.Active).Single();
            return ConvertDatabaseObject(dbResult);
        }

        public static List<TService> GetAll()
        {
            return DynamicSearch();
        }

        public static List<TService> DynamicSearch(string searchText = "")
        {
            using (var db = new RegistosRetroDB())
            {
                IQueryable<Services> query = db.Services.Where(x => x.Active);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.Trim();
                    if (decimal.TryParse(searchText, out decimal amount))
                        query = query.Where(x => x.Amount == amount);
                    else
                    {
                        string searchLower = searchText.ToLower();
                        query = query.Where(x => x.Service.ToLower().Contains(searchLower) || x.Reference.ToLower().Contains(searchLower));
                    }
                }

                var dbResult = query.ToList();
                return dbResult.Select(ConvertDatabaseObject).ToList();
            }
        }

        public static TService Add(string reference, string service, decimal amount)
        {
            if (Exists(service))
                return Get(service);
            var db = new RegistosRetroDB();
            var dbResult = new Services();
            dbResult.Service = service.Trim();
            dbResult.Reference = reference.Trim();
            dbResult.Amount = amount;
            dbResult.CreationDate = DateTime.Now;
            dbResult.Active = true;
            db.Services.Add(dbResult);
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static TService Update(int id, string reference, string service, decimal amount)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Services.Where(x => x.id == id).Single();
            if (dbResult.Service.Trim().ToLower() != service.Trim().ToLower())
                if (Exists(service))
                    throw new Exception("There is already a service with the name \"" + service.Trim() + "\".");

            dbResult.Service = service.Trim();
            dbResult.Reference = reference.Trim();
            dbResult.Amount = amount;
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static bool Exists(string service, int id = 0)
        {
            var db = new RegistosRetroDB();
            if (id <= 0)
                return db.Services.Where(x => x.Service.ToLower().Trim() == service.ToLower().Trim() && x.Active).Any();
            else
                return db.Services.Where(x => x.Service.ToLower().Trim() == service.ToLower().Trim() && x.Active && x.id != id).Any();
        }

        public static void Delete(int id)
        {
            var db = new RegistosRetroDB();
            var service = db.Services.Where(x => x.id == id).Single();
            service.Active = false;
            db.SaveChanges();
        }

        public static bool ExistsByReference(string reference, int id = 0)
        {
            var db = new RegistosRetroDB();
            if (id <= 0)
                return db.Services.Where(x => x.Reference.ToLower().Trim() == reference.ToLower().Trim() && x.Active).Any();
            else
                return db.Services.Where(x => x.Reference.ToLower().Trim() == reference.ToLower().Trim() && x.Active && x.id != id).Any();
        }

        public static TService GetByReference(string reference)
        {
            if (!ExistsByReference(reference))
                throw new Exception("Doesn't exist any service with the reference \"" + reference + "\"");
            
            var db = new RegistosRetroDB();
            var dbResult = db.Services.Where(x => x.Reference.Trim().ToLower() == reference.Trim().ToLower() && x.Active).Single();
            return ConvertDatabaseObject(dbResult);
        }
    }
}
