using SeferTasiDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeferTasiBLL.Repository
{
    public abstract class RepositoryBase<T,ID> where T:class
    {
        protected internal static MyContext dbContext;

        public virtual List<T> HepsiniGetir()
        {
            dbContext = new MyContext();
            return dbContext.Set<T>().ToList();
        }

        public virtual T IDYiGetir(ID id)
        {
            dbContext = new MyContext();
            return dbContext.Set<T>().Find(id);
        }

        public virtual void Ekle(T entity)
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                dbContext.Set<T>().Add(entity);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Sil(T entity)
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                dbContext.Set<T>().Remove(entity);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Guncelle()
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
