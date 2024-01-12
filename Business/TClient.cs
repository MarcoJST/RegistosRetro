using Database.RegistosRetro;
using System;
using System.Collections.Generic;

namespace Business
{
    public class TClient
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }

        public TClient() { 
            id = -1;
            Name = "";
            Address = "";
            Phone = "";
            Email = "";
            CreationDate = new DateTime();
        }

        private static TClient ConvertDatabaseObject(Clients dbObject)
        {
            return new TClient();
        }

        public static TClient Get(int id)
        {
            return new TClient();
        }

        public static List<TClient> GetAll()
        {
            return new List<TClient>();
        }

        public static TClient Add(string Name, string Address, string Phone, string Email)
        {
            return new TClient();
        }

        public static TClient Update(int id, string Name, string Address, string Phone, string Email)
        {
            return new TClient();
        }

        public static TClient Exists(string Name, string Address, string Phone, string Email)
        {
            return new TClient();
        }

        public static void Delete(int id)
        {
        }
    }
}
