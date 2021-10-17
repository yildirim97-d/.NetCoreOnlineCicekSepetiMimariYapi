using AppCore.DataAccess.EntityFramework.Bases;
using Entities.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityFramework.Repositories.Bases
{
    public abstract class ProductRepositoryBase: RepositoryBase<Product>
    {
        protected ProductRepositoryBase(DbContext db):base(db)
        {


        }
    }
}
