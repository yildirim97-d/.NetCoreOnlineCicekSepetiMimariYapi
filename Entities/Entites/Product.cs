using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entites
{
   public class Product : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public double UnitPrice { get; set; }
        public int  StockAmount { get; set; }

        //public DateTime? ExpiraionDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [StringLength(255)]
        public string ImageFileName { get; set; }
    }
}
