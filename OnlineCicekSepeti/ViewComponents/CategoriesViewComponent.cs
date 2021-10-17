using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCicekSepeti.ViewComponents
{
    
    public class CategoriesViewComponent  : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoriesViewComponent(ICategoryService categoryService)
        {

            _categoryService = categoryService;
        }

        public ViewViewComponentResult Invoke(int? categoryId)
        {
            List<CategoryModel> categories;
            Task<Result<List<CategoryModel>>> task = _categoryService.GetCategoriesAsync(); // *3
            Result<List<CategoryModel>> result = task.Result;
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.message);
            categories = result.Data;

            ViewBag.CategoryId = categoryId;
            return View(categories);
        }

    }



}
    
