using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entites
{
   public class Category : RecordBase
    {

        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }
}
