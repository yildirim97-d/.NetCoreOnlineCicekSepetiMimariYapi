using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class CityModel : RecordBase
    {
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [StringLength(200, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        [DisplayName("Şehir")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [DisplayName("Ülke")]
        public int CountryId { get; set; }

        public CountryModel Country { get; set; }
    }
}
