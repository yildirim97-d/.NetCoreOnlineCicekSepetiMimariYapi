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
using Business.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCicekSepeti.Controllers
{
 [Authorize(Roles ="Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
      

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public IActionResult Index()
        {
            var query = _categoryService.GetQuery();
            
            var model = query.ToList();
            return View(model);
        }

        
        
        // GET: Categories/Create
        public IActionResult Create()
        {
            var categoryModel = new CategoryModel();
            return View(categoryModel);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
               var result = _categoryService.Add(category);
                if (result.Status == ResultStatus.Succes)
                    return RedirectToAction(nameof(Index));
                if ( result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.message);
                }
                throw new Exception(result.message);
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var query = _categoryService.GetQuery();
            
            var category = query.SingleOrDefault(p => p.Id == id.Value);
            if(category == null)
                return View("NotFound");
           


            
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel category)
        {
            
            if (ModelState.IsValid)
            {
                var result = _categoryService.Update(category);
                if(result.Status == ResultStatus.Succes)
                return RedirectToAction(nameof(Index));
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.message);
                    return View(category);
                }
                throw new Exception(result.message);
                
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        

        // POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var deleteResult = _categoryService.Delete(id);
            if(deleteResult.Status == ResultStatus.Succes)
            return RedirectToAction(nameof(Index));

            if (deleteResult.Status == ResultStatus.Error)
            {
                ModelState.AddModelError("", deleteResult.message);
                var catquery = _categoryService.GetQuery();
               
                    var category = catquery.SingleOrDefault(c => c.Id == id);
                    return View("Edit",category);
                
               
            }
                
            throw new Exception(deleteResult.message);
        }
          
       
    }
}
