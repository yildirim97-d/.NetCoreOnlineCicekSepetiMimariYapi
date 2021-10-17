using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Business.Models
{
    public class CartModel
    {
       
        public int ProductId { get; set; }
        public int UserId { get; set; }
        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }
        [DisplayName("Ürün Fiyatı")]
        public double UnitPrice { get; set; }
        [DisplayName("Ürün Resim")]
        public string ProductImage { get; set; }
    }
}
