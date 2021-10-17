using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using DataAccess.EntityFramework.Repositories.Bases;
using Entities.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepositoryBase _categoryRepository;

        public CategoryService(CategoryRepositoryBase categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }



        public Result Add(CategoryModel model)
        {
            try
            {
                if (_categoryRepository.GetEntityQuery().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()))
                {
                    return new ErrorResult("Bu isimde zaten bir kategoriniz var!");
                }
                var entity = new Category()
                {
                    Description = model.Description?.Trim(),
                    Name = model.Name.Trim()
                };
                _categoryRepository.Add(entity);
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
                var category = _categoryRepository.GetEntityQuery(c => c.Id == id,"Products").SingleOrDefault();
                if(category.Products !=null && category.Products.Count > 0)
                {
                    return new ErrorResult("Kategoride ürün var, bu yüzden silinemez!");
                }
                _categoryRepository.Delete(category);
                return new SuccesResult();
            }
            catch (Exception e)
            {

                return new ExceptionResult(e);
            }
        }

        public void Dispose()
        {
            _categoryRepository.Dispose();
        }

        public IQueryable<CategoryModel> GetQuery()
        {
           
                var query = _categoryRepository.GetEntityQuery("Products").OrderBy(c => c.Name).Select(c => new CategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Guid = c.Guid,
                    ProductCount = c.Products.Count
                    
                    
                });
                return query;

           
        }

        public Result Update(CategoryModel model)
        {
            try
            {
                if (_categoryRepository.GetEntityQuery().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()&&c.Id!=model.Id ))
                {
                    return new ErrorResult("Bu isimde zaten bir kategoriniz var!");
                }
                var entity = _categoryRepository.GetEntityQuery(c => c.Id == model.Id).SingleOrDefault();
                
                    entity.Description = model.Description?.Trim();
                    entity.Name = model.Name.Trim();
                
                _categoryRepository.Update(entity);
                return new SuccesResult();
            }
            catch (Exception e)
            {

                return new ExceptionResult(e);
            }
        }




        public async Task<Result<List<CategoryModel>>> GetCategoriesAsync()
        {
            try
            {
                List<Category> categoryEntities = await _categoryRepository.GetEntityQuery().OrderBy(c => c.Name).ToListAsync();
                List<CategoryModel> categories = categoryEntities.Select(c => new CategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
                return new SuccesResult<List<CategoryModel>>(categories);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<CategoryModel>>(exc);
            }
        }


    }
}
