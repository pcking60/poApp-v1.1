using PostOffice.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("Transactions")]
    public class Transaction : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ServiceId { get; set; }

        public string UserId { get; set; }

        public int? Quantity { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTimeOffset TransactionDate { get; set; }
        
        public bool IsCash { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}