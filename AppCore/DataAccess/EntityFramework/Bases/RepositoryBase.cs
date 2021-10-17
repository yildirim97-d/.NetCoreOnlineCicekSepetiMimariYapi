using AppCore.Records.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AppCore.DataAccess.EntityFramework.Bases
{
    /*
     -abstract olmalı. Temel işlemler tanımlanıp heryerde kullanıcaz.
     -Interface yapılırsa heryerde implement edilmeli gerek yok.
     -Generic yapı olmalı class.<TEntity> . (Bu base sınıfını birden fazla yerde kullanacağımız için..
     -TEntity yerine bildiğimiz sınıf parametresi gelmeli ki ona göre işlem yapabilelim.
     -Mantıken bu class int string vs gibi şeyler almamalı class almalı sadece. Bunun için where TEntity:class demeliyiz.
     - Daha da özelleştirelim. Peki Bizim classlarımız sonuç olarak RecordBase den türeyecekler. ve onlarda birer class.
     . Bunun için where TEntity:class yerine gelin TEntity:RecordBase diyelim .
     */
    public abstract class RepositoryBase<TEntity> : IDisposable where TEntity : RecordBase, new()
    {
        private readonly DbContext _db;
        
        protected RepositoryBase(DbContext db)
        {
            _db = db;
        }
        /*
         Public virtual (virtual sebebi istediğini override yapabilsin) IQueryable
         mantık yine aynı generic tip vs. Şöyleki IQueryable<ÜrünModel> vs diyorduk ama bunları oluştururken elimde daha sınıfım yok.
         Bu yüzden Iqueryable<TEntity> demeliyimki sonrasında kullanırken sınıfımı parametre misali yazabileyim ve sistem öyle işlesin.(O tipte sorgu dönüş oluşturacak.)
         */
        //Params?? İlişkili modelleri çağrabiliyoruz.
        //Kullanımı :  productrepo.GetEntitiyQuery("Category","ProductStore") ..

        public virtual IQueryable<TEntity> GetEntityQuery(params string[] entitiesToInclude )

        {
            try
            {
                var query = _db.Set<TEntity>().AsQueryable();
                foreach(var entityToInclude in entitiesToInclude)
                {
               
                    
                    query = query.Include(entityToInclude);
                }
                return query;
            }
            catch (Exception e)
            {

                throw e;
            }
           

        }

        /*
         Expression nedir?:
         mesela GetEntityQuery(p=> p.Name.Contains("HP"),"Category","Product"; gibi sorgu özelliği sağlar..
         */

        public virtual IQueryable<TEntity> GetEntityQuery(Expression<Func<TEntity,bool>> pradicate, params string[] entitiesToInclude)
        {
            try
            {
                var query = GetEntityQuery(entitiesToInclude);
                return query.Where(pradicate);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        // Her eklemeden sonra SaveChanges i çağırmaktansa en son işlem sonunda çağırmamızı sağlayan bool mekanizması.
        public virtual void Add(TEntity entity, bool saveChanges = true)
        {
            try
            {
                _db.Set<TEntity>().Add(entity);
                if (saveChanges)
                    SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }

        }



        public virtual void Update (TEntity entity , bool saveChanges = true)
        {
            try
            {
                //_db.Entry(entity).State = EntityState.Modified;
                _db.Set<TEntity>().Update(entity);
                if (saveChanges)
                    SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public virtual void Delete (TEntity entity , bool saveChanges =true)
        {
            try
            {
                _db.Set<TEntity>().Remove(entity);
                if (saveChanges)
                    SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }
        }




        public virtual void Delete(int id, bool saveChanges = true)
        {
            try
            {
                var entity = GetEntityQuery(e => e.Id == id).SingleOrDefault();
                Delete(entity, saveChanges);
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public virtual void Delete(string guid, bool saveChanges = true)
        {
            try
            {
                var entity = GetEntityQuery(e => e.Guid == guid).SingleOrDefault();
                Delete(entity, saveChanges);
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        public virtual int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();

            }
            catch (Exception e)
            {

                throw e;
            }




        }




        #region
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            _db?.Dispose();
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
