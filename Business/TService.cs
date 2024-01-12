using Database.RegistosRetro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return new TService();
        }

        public static TService Get(int id)
        {
            return new TService();
        }

        public static List<TService> GetAll()
        {
            return new List<TService>();
        }

        public static TService Add(string Service, decimal Amount)
        {
            return new TService();
        }

        public static bool Exists(string Service)
        {
            return false;
        }

        public static TService Update(int id, string Service, decimal Amount)
        {
            return new TService();
        }

        public static void Delete(int id)
        {
        }
    }
}
