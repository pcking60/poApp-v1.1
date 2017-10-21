using PostOffice.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("TransactionDetails")]
    public class TransactionDetail : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int PropertyServiceId { get; set; }
        
        public decimal? Money { get; set; }

        [ForeignKey("TransactionId")]
        [Column(Order =1)]
        public virtual Transaction Transaction { get; set; }

        [ForeignKey("PropertyServiceId")]
        [Column(Order = 2)]
        public virtual PropertyService PropertyService { get; set; }
    }
}