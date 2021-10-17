using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class UserDetailModel:RecordBase
    {
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [StringLength(200, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        [DisplayName("E-Mail")]
        [EmailAddress(ErrorMessage = "{0} Yazım hatası!")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [DisplayName("Ülke")]
        public int CountryId { get; set; }

        public CountryModel Country { get; set; }

        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [DisplayName("Şehir")]
        public int CityId { get; set; }
        [DisplayName("Şehir")]
        public CityModel City { get; set; }
        [DisplayName("Adres")]
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [StringLength(1000, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        public string Address { get; set; }
    }
}
