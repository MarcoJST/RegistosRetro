namespace Database.RegistosRetro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Services
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Services()
        {
            InvoicesEntries = new HashSet<InvoicesEntries>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string Service { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreationDate { get; set; }

        public bool Active { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoicesEntries> InvoicesEntries { get; set; }
    }
}
