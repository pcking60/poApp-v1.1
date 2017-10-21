using PostOffice.Model.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("PostOffices")]
    public class PO : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int Code { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public int? POStyle { get; set; }

        [MaxLength(500)]
        public string POAddress { get; set; }

        [MaxLength(50)]
        public string POMobile { get; set; }

        [Required]
        public int DistrictID { get; set; }

        [ForeignKey("DistrictID")]
        public virtual District District { get; set; }

        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}