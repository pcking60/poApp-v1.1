using System.ComponentModel.DataAnnotations;

namespace PostOffice.Model.Models
{
    public class Period
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }
    }
}