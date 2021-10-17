using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using DataAccess.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public interface ICityService : IServices<CityModel>
    {
        Result<List<CityModel>> GetCities(int? countryId= null);
    }


    public class CityService : ICityService
    {
        private readonly CityRepositoryBase _cityReposityBase;
        public CityService(CityRepositoryBase cityReposityBase)
        {
            _cityReposityBase = cityReposityBase;
        }
        public Result Add(CityModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _cityReposityBase?.Dispose();
        }

        public IQueryable<CityModel> GetQuery()
        {
            return _cityReposityBase.GetEntityQuery("Country").OrderBy(c => c.Name).Select(c => new CityModel()
            {
                Id = c.Id,
                Guid = c.Guid,
                Name = c.Name,
                CountryId = c.CountryId,
                Country = new CountryModel()
                {
                    Id = c.Country.Id,
                    Guid = c.Country.Guid,
                    Name = c.Country.Name
                }
            });
        }
        public Result<List<CityModel>> GetCities(int? countryId = null)
        {
            try
            {
                var list = GetQuery();
                if (countryId.HasValue)
                        list = list.Where(c => c.CountryId == countryId.Value);
                return new SuccesResult<List<CityModel>>(list.ToList());
            }
            catch (Exception e)
            {

                return new ExceptionResult<List<CityModel>>(e);
            }
           

           
        }

        public Result Update(CityModel model)
        {
            throw new NotImplementedException();
        }
    }
}
