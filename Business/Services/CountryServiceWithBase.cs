using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Enums;
using Business.Models;
using DataAccess.EntityFramework.Repositories;
using Entities.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public interface ICountryService : IServices<CountryModel>
    {
        Result<List<CountryModel>> GetCountries();
    }
    public class CountryService : ICountryService
    {
        private readonly CountryRepositoryBase _countryRepository;
        public CountryService(CountryRepositoryBase countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public Result Add(CountryModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _countryRepository.Dispose();
        }

        public IQueryable<CountryModel> GetQuery()
        {
            return _countryRepository.GetEntityQuery().Select(c => new CountryModel()
            {
                Id=c.Id,
                Name= c.Name,
                Guid = c.Guid,
                CountryId = c.Id
                
            });
        }

        public Result Update(CountryModel model)
        {
            throw new NotImplementedException();
        }
        public Result<List<CountryModel>> GetCountries()
        {
            try
            {
                var countries = GetQuery().ToList();
                return new SuccesResult<List<CountryModel>>(countries);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<CountryModel>>(exc);
            }
        }
    }
}
