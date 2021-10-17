using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entites
{
    public class Role : RecordBase
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
