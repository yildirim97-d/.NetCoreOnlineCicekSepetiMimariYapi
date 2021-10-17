using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models.Reports
{
    public class ProductsReportModel : RecordBase
    {
        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }
        [DisplayName("Ürün Açıklama")]
        public string ProductDescription { get; set; }

        [DisplayName("Kategori Adı")]
        public string CategoryName { get; set; }
        [DisplayName("Kategori Açıklama")]
        public string CategoryDescription { get; set; }

        [DisplayName("Ürün Fiyat")]
        public string UnitPriceText { get; set; }
        [DisplayName("Ürün Stok")]
        public int StockAmount { get; set; }

        public CategoryModel Category { get; set; }

        [DisplayName("Kategori Adı")]
        public int CategoryId { get; set; }
        [DisplayName("Ürün Fiyat")]
        public double UnitPrice { get; set; }

        [StringLength(255)]
        [DisplayName("Resim Seç")]
        public string ImageFileName { get; set; }


    }
}
