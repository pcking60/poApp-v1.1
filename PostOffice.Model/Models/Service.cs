using PostOffice.Model.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("Services")]
    public class Service : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Alias { get; set; }

        [Required]
        public int GroupID { get; set; }        

        [MaxLength(50)]        
        public string Unit { get; set; }

        public decimal? BuyIn { get; set; }
        public decimal? SoldOut { get; set; }
        public float? VAT { get; set; }
        public int? PayMethodID { get; set; }

        [ForeignKey("PayMethodID")]
        [Column(Order =1)]
        public virtual IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [ForeignKey("GroupID")]
        [Column(Order = 2)]
        public virtual ServiceGroup ServiceGroup { get; set; }

        
        public virtual IEnumerable<PropertyService> PropertyServices { get; set; }
    }
}