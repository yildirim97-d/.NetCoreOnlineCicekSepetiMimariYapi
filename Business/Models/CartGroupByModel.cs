using Business.Models.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Text;

namespace Business.Models
{
    public class CartGroupByModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }
        
        [DisplayName("Ürün Toplam Fiyatı")]
        public string TotalUnitPrice { get; set; }
       
        [DisplayName("Ürün Sayısı")]
        public int TotalCount { get; set; }
        [DisplayName("Ürün Resim")]
        public string ProductImage { get; set; }

    }
}
