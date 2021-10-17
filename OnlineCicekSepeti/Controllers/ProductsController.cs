using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.EntityFramework.Contexts;
using Entities.Entites;
using Business.Services.Bases;
using AppCore.Business.Models.Results;
using Microsoft.Extensions.DependencyInjection;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Business.Models.Filters;
using OnlineCicekSepeti.Models;
using AppCore.Business.Models.Ordering;
using Microsoft.AspNetCore.Http;
using OnlineCicekSepeti.Settings;
using System.IO;
using Business.Models.Reports;
using System.Text.RegularExpressions;

namespace OnlineCicekSepeti.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
      

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        //public ProductsController(ECicekSepetiContext context)
        //{
        //    _context = context;
        //}

        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    var eCicekSepetiContext = _context.Products.Include(p => p.Category);
        //    return View(await eCicekSepetiContext.ToListAsync());
        //}

        public IActionResult Index(int? categoryId)
        {
            
            var productsFilter = new ProductsReportFilterModel()
            {
                CategoryId = categoryId
            };

            
          
            var order = new OrderModel()
            {
                Expression = "Category Name",
                DirectionAscending = true
            };
            

            var result = _productService.GetProductsReport(productsFilter, order);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.message);
            var productsReport = result.Data;

            

            var viewModel = new ProductsReportAjaxIndexViewModel()
            {
                ProductsReport = productsReport,
                ProductsFilter = productsFilter,


                OrderByExpression = order.Expression,
                OrderByDirectionAscending = order.DirectionAscending

                
            };

           
            if (productsReport.Count <= 0)
            {

                var category = _categoryService.GetQuery().SingleOrDefault(u => u.Id == categoryId.Value);
                ViewData["NoProduct"] =  category.Name+" kategorisine ait ürün bulunmamaktadır.";
                
                return View(viewModel);
            }
           
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Index(ProductsReportAjaxIndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
             
                var order = new OrderModel()
                {
                    Expression = viewModel.OrderByExpression,
                    DirectionAscending = viewModel.OrderByDirectionAscending
                };
             

                var result = _productService.GetProductsReport(viewModel.ProductsFilter, order);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.message);
                viewModel.ProductsReport = result.Data;




            }



            return View(viewModel);
        }





        // GET: Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}



        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var query = _productService.GetQuery();
           

            var model = query.SingleOrDefault(p => p.Id == id.Value);
            if(model == null)
            {
                return View("NotFound");
            }
            return View(model);
            

        




        }











        // GET: Products/Create
        //public IActionResult Create()
        //{
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        //    return View();
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var query = _categoryService.GetQuery();
           
            ViewBag.Categories = new SelectList(query.ToList(),"Id","Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Create([Bind("Name,Description,UnitPrice,StockAmount,ExpiraionDate,CategoryId,Id,Guid")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}


        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductsReportModel product, IFormFile image)
        {
            Result productResult;
            IQueryable<CategoryModel> categoryQuery;
            if (ModelState.IsValid)
            {



                string fileName = null;
                string fileExtension = null;
                string filePath = null; 
                bool saveFile = false; 
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName; 
                    fileExtension = Path.GetExtension(fileName); 
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false; 
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "Resim uzantısı yanlı.İzin verilen resim uzantısı " + AppSettings.AcceptedImageExtensions);
                        categoryQuery = _categoryService.GetQuery();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                        return View(product);
                    }

                   
                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2); // bytes
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "Resim boyutu çok büyük. İzin verilen resim uzatnısı " + AppSettings.AcceptedImageMaximumLength + " MB olmalıdır.");
                        categoryQuery = _categoryService.GetQuery();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                        return View(product);
                    }

                    saveFile = true;
                }
                if (saveFile)
                {
                    fileName = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "") + fileExtension; 

                    filePath = Path.Combine("wwwroot", "files", "products", fileName); 
                   
                }
                product.ImageFileName = fileName;







                productResult = _productService.Add(product);
                if (productResult.Status == ResultStatus.Exception)
                    throw new Exception(productResult.message);
                if(productResult.Status == ResultStatus.Succes)
                {
                    if(saveFile)
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.CreateNew))
                        {
                            image.CopyTo(fileStream);
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }



                ModelState.AddModelError("", productResult.message);
                categoryQuery = _categoryService.GetQuery();
                
                ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                return View(product);



            }
            categoryQuery = _categoryService.GetQuery();
            
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }









        // GET: Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}


        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View("NotFound");
            var productQuery = _productService.GetQuery();
           
            var product = productQuery.SingleOrDefault(p => p.Id == id.Value);
            if(product == null)
                return View("NotFound");
            var categoryQuery = _categoryService.GetQuery();
           
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name",product.CategoryId);
            return View(product);
        }












        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Name,Description,UnitPrice,StockAmount,ExpiraionDate,CategoryId,Id,Guid")] Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}




        public IActionResult Edit(ProductsReportModel product, IFormFile image)
        {
            Result productResult;
            IQueryable<CategoryModel> categoryQuery;
            if (ModelState.IsValid)
            {





                string fileName = null;
                string fileExtension = null;
                string filePath = null;
                bool saveFile = false;
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName;
                    fileExtension = Path.GetExtension(fileName);
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false;
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "Resim uzantısı yanlı.İzin verilen resim uzantısı " + AppSettings.AcceptedImageExtensions);
                        categoryQuery = _categoryService.GetQuery();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                        return View(product);
                    }

                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2); 
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "Resim boyutu çok büyük. İzin verilen resim uzatnısı " + AppSettings.AcceptedImageMaximumLength + " MB olmalıdır.");
                        categoryQuery = _categoryService.GetQuery();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                        return View(product);
                    }

                    saveFile = true;
                }


                var existingProduct = _productService.GetQuery().SingleOrDefault(p => p.Id == product.Id);

                if (saveFile) 
                {
                    if (string.IsNullOrWhiteSpace(existingProduct.ImageFileName)) 
                    {
                        fileName = Guid.NewGuid() + fileExtension; 
                    }
                    else 
                    {
                        
                        int periodIndex = existingProduct.ImageFileName.IndexOf("."); 
                        fileName = existingProduct.ImageFileName.Substring(0, periodIndex); 
                        string existingProductImageFileExtension = existingProduct.ImageFileName.Substring(periodIndex); 
                        if (existingProductImageFileExtension != fileExtension) 
                        {
                           
                            filePath = Path.Combine("wwwroot", "files", "products", existingProduct.ImageFileName);
                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);
                        }
                        fileName = fileName + fileExtension;
                    }
                }
                else 
                {
                    fileName = existingProduct.ImageFileName;
                }

                product.ImageFileName = fileName;
















                productResult = _productService.Update(product);
                if (productResult.Status == ResultStatus.Exception)
                    throw new Exception(productResult.message);
                if (productResult.Status == ResultStatus.Succes)
                {
                    if (saveFile)
                    {
                        filePath = Path.Combine("wwwroot", "files", "products", fileName);
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }


               

                ModelState.AddModelError("", productResult.message);
                categoryQuery = _categoryService.GetQuery();
                
                ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                return View(product);



            }
            categoryQuery = _categoryService.GetQuery();
           
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }
















        // GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return View("NotFound");
            var existingProduct = _productService.GetQuery().SingleOrDefault(p => p.Id == id.Value);
            var result = _productService.Delete(id.Value);
            if (result.Status == ResultStatus.Succes)
                if (!string.IsNullOrWhiteSpace(existingProduct.ImageFileName))
                {
                    string filePath = Path.Combine("wwwroot", "files", "products", existingProduct.ImageFileName);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            return RedirectToAction(nameof(Index));
            throw new Exception(result.message);

        }




        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProductImage(int? id)
        {
            if (id == null)
                return View("NotFound");

            var existingProduct = _productService.GetQuery().SingleOrDefault(p => p.Id == id.Value);
            if (!string.IsNullOrWhiteSpace(existingProduct.ImageFileName))
            {
                string filePath = Path.Combine("wwwroot", "files", "products", existingProduct.ImageFileName);
                existingProduct.ImageFileName = null;
                var result = _productService.Update(existingProduct);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.message);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            return View(nameof(Details), existingProduct);
        }





    }
}
