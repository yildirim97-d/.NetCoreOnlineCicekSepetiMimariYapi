using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entites
{
   public class Country:RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public List<City> Cities { get; set; }
        public List<UserDetail> UserDetail { get; set; }

    }
}
