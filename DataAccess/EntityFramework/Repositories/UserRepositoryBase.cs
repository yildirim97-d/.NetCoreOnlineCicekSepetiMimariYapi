using AppCore.DataAccess.EntityFramework.Bases;
using Entities.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityFramework.Repositories
{
    public abstract class UserRepositoryBase : RepositoryBase<User>

    {
        protected UserRepositoryBase(DbContext db) : base(db)
        {

        }
        
    }
    public class UserReposity : UserRepositoryBase
    {
        public UserReposity(DbContext db) : base(db)
        {

        }
    }
}
