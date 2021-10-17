using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entites
{
   public class City:RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<UserDetail> UserDetail { get; set; }
    }
}
