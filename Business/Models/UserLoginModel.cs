using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class UserLoginModel
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



    }
}
