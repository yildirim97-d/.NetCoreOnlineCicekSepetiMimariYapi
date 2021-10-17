using AppCore.Records.Bases;
using Business.Models.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Business.Models
{
   public class ProductModel:RecordBase
    {
        public ProductModel()
        {

            OrderByDirectionAscending = true;
        }
        [Required(ErrorMessage ="{0} Boş geçilemez!")]
        [MinLength(5,ErrorMessage ="{0} En az {1} karakter olmalı!")]
        [MaxLength(200, ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        [DisplayName("Ürün Adı")]
        public string Name { get; set; }
        [DisplayName("Açıklama")]

        [StringLength(500,ErrorMessage = "{0} En fazla {1} karakter olmalı!")]
        public string Description { get; set; }

        [DisplayName("Ürün Fiyatı")]
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        public double UnitPrice { get; set; }

        [DisplayName("Ürün Fiyatı")]
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        public string UnitPriceText { get; set; }

        //public string UnitPriceText => UnitPrice.ToString(new CultureInfo("en"));

        [DisplayName("Ürün Stok")]

       
        public int StockAmount { get; set; }

        [DisplayName("Kategori")]
        [Required(ErrorMessage = "{0} Boş geçilemez!")]
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        [StringLength(255)]
        public string ImageFileName { get; set; }


       

        public List<ProductModel> ProductsReport { get; set; }
        public ProductsReportFilterModel ProductsFilter { get; set; }
        public SelectList Categories { get; set; }





        // Sıralama
        public string OrderByExpression { get; set; }
        public bool OrderByDirectionAscending { get; set; }
    }
}
