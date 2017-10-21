using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("TransactionTypes")]
    public class TransactionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(256)]
        [Required]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }
    }
}