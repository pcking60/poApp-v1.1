using PostOffice.Model.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("Districts")]
    public class District : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int Code { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public virtual IEnumerable<PO> PostOffices { get; set; }
    }
}