namespace Database.RegistosRetro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoicesEntries
    {
        public int id { get; set; }

        public int Invoice { get; set; }

        public int Service { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Local { get; set; }

        public decimal Amount { get; set; }

        public decimal Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime ServiceDate { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual Invoices Invoices { get; set; }

        public virtual Services Services { get; set; }
    }
}
