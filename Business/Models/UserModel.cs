using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class UserModel : RecordBase
    {
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [MinLength(3, ErrorMessage = "{0} En az {1} karakter olmalı!")]
        [MaxLength(30, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [MinLength(3, ErrorMessage = "{0} En az {1} karakter olmalı!")]
        [MaxLength(10, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        [DisplayName("Şifre")]
        public string Password { get; set; }
        [DisplayName("Şifre Tekrarı")]
        [Compare("Password",ErrorMessage = "{1} ve {0} aynı olmalı ! ")]
        public string ConfirmPassword { get; set; }

        public bool active { get; set; }

        [DisplayName("Active")]
        public string ActiveText { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        public int RoleId { get; set; }

        public RoleModel Role { get; set; }

        public int UserDetailId { get; set; }
        public UserDetailModel UserDetail { get; set; }
    }
}
