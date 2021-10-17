using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class RoleModel : RecordBase
    {
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [StringLength(50, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        [DisplayName("Role")]
        public string Name { get; set; }

        public List<UserModel> Users { get; set; }
    }
}
