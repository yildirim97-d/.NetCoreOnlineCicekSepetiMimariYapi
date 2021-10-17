using AppCore.DataAccess.EntityFramework.Bases;
using Entities.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityFramework.Repositories
{
    public abstract class CountryRepositoryBase : RepositoryBase<Country>

    {
        protected CountryRepositoryBase(DbContext db) : base(db)
        {

        }
        
    }
    public class CountryReposity : CountryRepositoryBase
    {
        public CountryReposity(DbContext db) : base(db)
        {

        }
    }
}
