using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class CategoryModel : RecordBase
    {

        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        [MinLength(2, ErrorMessage = "{0} En az {1} karakter olmalı!")]
        [MaxLength(200, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        [DisplayName("Kategori Adı")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        [DisplayName("Kategori Açıklama")]
        public string Description { get; set; }
        [DisplayName("Ürün Sayısı")]
        public int ProductCount{ get; set; }
    }
}
