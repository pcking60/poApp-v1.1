using PostOffice.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("PropertyServices")]
    public class PropertyService : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DataType("nvarchar")]
        [MaxLength(128)]
        public string Name { get; set; }
        
        public decimal? Percent { get; set; }

        [Required]
        public int ServiceID { get; set; }

        [ForeignKey("ServiceID")]
        public virtual Service Service { get; set; }

        
    }
}