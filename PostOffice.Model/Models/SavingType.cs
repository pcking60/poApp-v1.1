using System.ComponentModel.DataAnnotations;

namespace PostOffice.Model.Models
{
    public class SavingType
    {
        [Key]
        public string Id { get; set; }
        [StringLength(256)]
        public string Description { get; set; }
    }
}