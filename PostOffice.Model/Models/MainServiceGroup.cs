using PostOffice.Model.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("MainServiceGroups")]
    public class MainServiceGroup : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataType("nvarchar")]
        [MaxLength(256)]
        public string Name { get; set; }

        public IEnumerable<ServiceGroup> ServiceGroups { get; set; }

       
    }
}