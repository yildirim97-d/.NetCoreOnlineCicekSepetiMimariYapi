using DataAccess.EntityFramework.Repositories.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityFramework.Repositories
{
    public class ProductRepository:ProductRepositoryBase
    {
        public ProductRepository(DbContext db):base(db)
        {
                
        }
    }
}
