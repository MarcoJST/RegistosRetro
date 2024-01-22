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
    public class TService
    {
        public int id { get; set; }
        public string Service { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreationDate { get; set; }

        public TService()
        {
            id = -1;
            Service = "";
            Amount = 0.00m;
            CreationDate = new DateTime();
        }

        private static TService ConvertDatabaseObject(Services dbObject)
        {
            var result = new TService();
            result.id = dbObject.id;
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
            var db = new RegistosRetroDB();
            var result = new List<TService>();
            var dbResult = db.Services.Where(x => x.Active).ToList();
            foreach (var item in dbResult)
                result.Add(ConvertDatabaseObject(item));
            return result;
        }

        public static TService Add(string service, decimal amount)
        {
            if (Exists(service))
                return Get(service);
            var db = new RegistosRetroDB();
            var dbResult = new Services();
            dbResult.Service = service.Trim();
            dbResult.Amount = amount;
            dbResult.CreationDate = DateTime.Now;
            dbResult.Active = true;
            db.Services.Add(dbResult);
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static TService Update(int id, string service, decimal amount)
        {
            var db = new RegistosRetroDB();
            var dbResult = db.Services.Where(x => x.id == id).Single();
            if (dbResult.Service.Trim().ToLower() != service.Trim().ToLower())
                if (Exists(service))
                    throw new Exception("There is already a service with the name \"" + service.Trim() + "\".");

            dbResult.Service = service.Trim();
            dbResult.Amount = amount;
            db.SaveChanges();
            return ConvertDatabaseObject(dbResult);
        }

        public static bool Exists(string service)
        {
            var db = new RegistosRetroDB();
            return db.Services.Where(x => x.Service.ToLower().Trim() == service.ToLower().Trim() && x.Active).Any();
        }

        public static void Delete(int id)
        {
            var db = new RegistosRetroDB();
            var service = db.Services.Where(x => x.id == id).Single();
            service.Active = false;
            db.SaveChanges();
        }
    }
}
