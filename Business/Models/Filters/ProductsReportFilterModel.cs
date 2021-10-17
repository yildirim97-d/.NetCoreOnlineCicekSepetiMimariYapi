using AppCore.Business.Services.Validations;
using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Business.Models.Filters
{
    public class ProductsReportFilterModel : RecordBase
    {
        [DisplayName("Kategori")]
        public int? CategoryId { get; set; }

        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }

        [DisplayName("Ürün Fiyatı")]
        [StringDecimal]
        public string UnitPriceBeginText { get; set; }

        [StringDecimal]
        public string UnitPriceEndText { get; set; }

   





    }
}
