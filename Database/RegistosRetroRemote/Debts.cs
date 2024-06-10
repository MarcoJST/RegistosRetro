namespace Database.RegistosRetroRemote
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Debts
    {
        public int id { get; set; }

        public int idClient { get; set; }

        [Required]
        [StringLength(200)]
        public string NameClient { get; set; }

        public decimal Debt { get; set; }

        public DateTime Date { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Email { get; set; }
    }
}
