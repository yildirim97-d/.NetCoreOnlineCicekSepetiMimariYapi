using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;
using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using Business.Models.Filters;
using Business.Models.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services.Bases
{
    public interface IProductService: IServices<ProductsReportModel>
    {
        Result<List<ProductsReportModel>> GetProductsReport(ProductsReportFilterModel filter,  OrderModel order = null);
    }
}
