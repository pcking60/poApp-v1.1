using PostOffice.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("TKBDAmounts")]
    public class TKBDAmount : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Month { get; set; }       
        public int Year { get; set; }
        public string Account { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal Amount { get; set; }
    }
}