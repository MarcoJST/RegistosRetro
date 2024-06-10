namespace Database.RegistosRetro
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class InvoicePayments
    {
        public int id { get; set; }

        public int Invoice { get; set; }

        public decimal Amount { get; set; }

        [StringLength(200)]
        public string Observations { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual Invoices Invoices { get; set; }
    }
}
