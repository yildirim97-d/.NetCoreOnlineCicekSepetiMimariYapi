using AppCore.Records.Bases;
using Business.Models.Filters;
using Business.Models.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCicekSepeti.Models
{
    public class ProductsReportAjaxIndexViewModel 
    {
        public ProductsReportAjaxIndexViewModel()
        {
           
            OrderByDirectionAscending = true;
        }

        public List<ProductsReportModel> ProductsReport { get; set; }
        public ProductsReportFilterModel ProductsFilter { get; set; }
        public SelectList Categories { get; set; }

       
      
        

        // Sıralama
        public string OrderByExpression { get; set; }
        public bool OrderByDirectionAscending { get; set; }
       
    }
}
