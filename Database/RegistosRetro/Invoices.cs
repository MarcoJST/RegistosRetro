namespace Database.RegistosRetro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Invoices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoices()
        {
            InvoicePayments = new HashSet<InvoicePayments>();
            InvoicesEntries = new HashSet<InvoicesEntries>();
        }

        public int id { get; set; }

        public int Client { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual Clients Clients { get; set; }

        public bool Active { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoicePayments> InvoicePayments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoicesEntries> InvoicesEntries { get; set; }
    }
}
