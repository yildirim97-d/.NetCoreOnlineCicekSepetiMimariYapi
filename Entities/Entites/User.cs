using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entites
{
    public class User: RecordBase
    {
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }
        [Required]
        [StringLength(15)]
        public string Password { get; set; }
        public bool active { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int UserDetailId { get; set; }
        public UserDetail UserDetail { get; set; }
    }
}
