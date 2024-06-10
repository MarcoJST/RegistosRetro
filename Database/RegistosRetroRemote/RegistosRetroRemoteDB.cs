using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace Database.RegistosRetroRemote
{
    public partial class RegistosRetroRemoteDB : DbContext
    {
        private static string conn = @"data source=" + ConfigurationManager.AppSettings["RegistosRetroRemoteDB"].ToString();
        public RegistosRetroRemoteDB() : base(conn)
        {
        }

        public virtual DbSet<Debts> Debts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
