using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Bases
{
   public interface ICategoryService: IServices<CategoryModel>
    {
        Task<Result<List<CategoryModel>>> GetCategoriesAsync();
    }
}
