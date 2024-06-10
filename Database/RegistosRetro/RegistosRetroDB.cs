using System.Configuration;
using System.Data.Entity;

namespace Database.RegistosRetro
{
    public partial class RegistosRetroDB : DbContext
    {
        private static string conn = @"data source=" + ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public RegistosRetroDB() : base(conn)
        {
        }

        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<InvoicePayments> InvoicePayments { get; set; }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<InvoicesEntries> InvoicesEntries { get; set; }
        public virtual DbSet<Services> Services { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.Clients)
                .HasForeignKey(e => e.Client);

            modelBuilder.Entity<Invoices>()
                .HasMany(e => e.InvoicePayments)
                .WithRequired(e => e.Invoices)
                .HasForeignKey(e => e.Invoice);

            modelBuilder.Entity<Invoices>()
                .HasMany(e => e.InvoicesEntries)
                .WithRequired(e => e.Invoices)
                .HasForeignKey(e => e.Invoice);

            modelBuilder.Entity<Services>()
                .HasMany(e => e.InvoicesEntries)
                .WithRequired(e => e.Services)
                .HasForeignKey(e => e.Service);
        }
    }
}
