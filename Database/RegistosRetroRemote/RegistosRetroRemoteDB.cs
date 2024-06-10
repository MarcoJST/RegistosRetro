using System.Configuration;
using System.Data.Entity;

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
