using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataAccess.EntityFramework.Repositories.Bases;
using System.Globalization;
using Entities.Entites;
using Business.Models.Reports;
using Business.Models.Filters;
using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepositoryBase _productRepository;
        private readonly CategoryRepositoryBase _categoryRepository;

        public ProductService(ProductRepositoryBase productRepository, CategoryRepositoryBase categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public Result Add(ProductsReportModel model)
        {
            try
            {
                if (_productRepository.GetEntityQuery().Any(p => p.Name.ToUpper() == model.ProductName.ToUpper().Trim()))
                    return new ErrorResult("Aynı üründen isim mevcut !");
                double unitPrice;
                //if (!double.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out unitPrice)) ;
                if (!double.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice))
                {
                    return new ErrorResult("Fiyat rakamlardan oluşmalıdır!");


                }
                model.UnitPrice = unitPrice;
                var entity = new Product()
                {
                    CategoryId = model.CategoryId,
                    Description = model.ProductDescription?.Trim(),
                    Name = model.ProductName.Trim(),
                    StockAmount = model.StockAmount,
                    UnitPrice = model.UnitPrice,
                    ImageFileName = model.ImageFileName

                };
                _productRepository.Add(entity);
                return new SuccesResult();
            }
            catch (Exception e)
            {

                return new ExceptionResult(e);
            }

        }

        public Result Delete(int id)
        {
            try
            {
                _productRepository.Delete(id);
                return new SuccesResult();

            }
            catch (Exception e)
            {

                return new ExceptionResult(e);
            }
        }

        public void Dispose()
        {
            _productRepository.Dispose();
        }

        public IQueryable<ProductsReportModel> GetQuery()
        {

            var query = _productRepository.GetEntityQuery("Category").OrderBy(p => p.Name).Select(p => new ProductsReportModel()

            {
                Id = p.Id,
                Guid = p.Guid,
                ProductName = p.Name,
                UnitPrice = p.UnitPrice,
                UnitPriceText = p.UnitPrice.ToString(new CultureInfo("en")),
                StockAmount = p.StockAmount,
                ProductDescription = p.Description,
                ImageFileName = p.ImageFileName,
                CategoryId = p.CategoryId,
                Category = new CategoryModel()
                {
                    Id = p.Category.Id,
                    Guid = p.Category.Guid,
                    Description = p.Description,
                    Name = p.Category.Name

                }
            });
            return query;




        }

        public Result Update(ProductsReportModel model)
        {
            try
            {
                if (_productRepository.GetEntityQuery().Any(p => p.Name.ToUpper() == model.ProductName.ToUpper().Trim() && p.Id != model.Id))
                    return new ErrorResult("Aynı üründen isim mevcut!");
                double unitPrice;
               
                if (!double.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice))
                {
                    return new ErrorResult("Fiyat rakamlardan oluşmalıdır!");


                }
                model.UnitPrice = unitPrice;
                var entity = _productRepository.GetEntityQuery(p => p.Id == model.Id).SingleOrDefault();

                entity.CategoryId = model.CategoryId;
                entity.Description = model.ProductDescription?.Trim();
                entity.Name = model.ProductName.Trim();
                entity.StockAmount = model.StockAmount;
                entity.UnitPrice = model.UnitPrice;
                entity.ImageFileName = model.ImageFileName;

                _productRepository.Update(entity);
                return new SuccesResult();
            }
            catch (Exception e)
            {

                return new ExceptionResult(e);
            }
        }






        public Result<List<ProductsReportModel>> GetProductsReport(ProductsReportFilterModel filter, OrderModel order = null)

        {
            try
            {
               
                var productQuery = _productRepository.GetEntityQuery();
                var categoryQuery = _categoryRepository.GetEntityQuery();

              
                var query = productQuery.Join(categoryQuery,
                    p => p.CategoryId,
                    c => c.Id,
                    (p, c) => new ProductsReportModel()
                    {
                        Id = p.Id,
                        CategoryDescription = c.Description,
                        CategoryName = c.Name,

                        ProductDescription = p.Description,
                        ProductName = p.Name,

                        
                        UnitPriceText = "" + p.UnitPrice.ToString(new CultureInfo("tr")), 
                        CategoryId = c.Id,
                        UnitPrice = p.UnitPrice,
                        ImageFileName = p.ImageFileName

                    }) ;
               
                query = query.OrderBy(q => q.CategoryName).ThenBy(q => q.ProductName); 
               
                if (order != null && !string.IsNullOrWhiteSpace(order.Expression))
                {
                    switch (order.Expression)
                    {
                        case "Ürün Adı":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.ProductName)
                                : query.OrderByDescending(q => q.ProductName);
                            break;
                        case "Kategori Adı":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.CategoryName)
                                : query.OrderByDescending(q => q.CategoryName);
                            break;
                        case "Ürün Fiyat":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.UnitPrice)
                                : query.OrderByDescending(q => q.UnitPrice);
                            break;
                        
                           
                       
                    }
                }
               

               
                if (filter.CategoryId.HasValue)
                    query = query.Where(q => q.CategoryId == filter.CategoryId.Value);
                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                {
                    
                    query = query.Where(q => q.ProductName.ToUpper().Contains(filter.ProductName.ToUpper().Trim()));
                }
                if (!string.IsNullOrWhiteSpace(filter.UnitPriceBeginText))
                {
                    double unitPriceBegin = Convert.ToDouble(filter.UnitPriceBeginText.Replace(",", "."),
                        CultureInfo.InvariantCulture);
                    query = query.Where(q => q.UnitPrice >= unitPriceBegin);
                }
                if (!string.IsNullOrWhiteSpace(filter.UnitPriceEndText))
                {
                    double unitPriceEnd = Convert.ToDouble(filter.UnitPriceEndText.Replace(",", "."),
                        CultureInfo.InvariantCulture);
                    query = query.Where(q => q.UnitPrice <= unitPriceEnd);
                }
               

               

                return new SuccesResult<List<ProductsReportModel>>(query.ToList());
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<ProductsReportModel>>(exc);
            }
        }





















    }
}
