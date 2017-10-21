using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    public class InterestRate
    {
        [Key]
        [Column(Order =0)]
        public string PeriodId { get; set; }
        [Key]
        [Column(Order = 1)]
        public string InterestTypeId { get; set; }
        [Key]
        [Column(Order = 2)]        
        public string SavingTypeId { get; set; }
        [ForeignKey("PeriodId")]
        public virtual Period Period { get; set; }
        [ForeignKey("InterestTypeId")]
        public virtual InterestType InterestType { get; set; }
        [ForeignKey("SavingTypeId")]
        public virtual SavingType SavingType { get; set; }
        public decimal Percent { get; set; }

    }
}