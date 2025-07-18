using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class Repo<TEntity, TContext> : IEntityDal<TEntity> where TEntity : class, IDataBaseEntity, new()
     where TContext : DbContext, new()
    {

        // asenkron programlama şu demektir : art arda uzun sürebilecek işlemleri bekletmek için fonksiyonlar
        // asenkron yazılabilir.
        public async Task<bool> Add(TEntity entity)
        {
            using (TContext db = new TContext())
            {
                // Entry çok genel bir tanım. Bu entry silme , ekleme , update hepsi olabilir.
                var adding_entity = db.Entry(entity);

                // burada bize gelen veriyi ne yaptıgımızı belirliyoruz
                adding_entity.State = EntityState.Added;


                int result = await db.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> Delete(TEntity entity)
        {
            using (TContext db = new TContext())
            {
                // Entry çok genel bir tanım. Bu entry silme , ekleme , update hepsi olabilir.
                var adding_entity = db.Entry(entity);

                // burada bize gelen veriyi ne yaptıgımızı belirliyoruz
                adding_entity.State = EntityState.Deleted;


                int result = await db.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> Edit(TEntity entity)
        {
            using (TContext db = new TContext())
            {
                // Entry çok genel bir tanım. Bu entry silme , ekleme , update hepsi olabilir.
                var adding_entity = db.Entry(entity);

                // burada bize gelen veriyi ne yaptıgımızı belirliyoruz
                adding_entity.State = EntityState.Modified;


                int result = await db.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<List<TEntity>> List(params Expression<Func<TEntity, object>>[] includes)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        // Metota id gelecek, id'ye göre tek bir ögeyi bulduracaksınız.
        public async Task<TEntity> GetbyId(int id)
        {
            using (TContext db = new TContext())
            {
                return db.Set<TEntity>().Find(id);
            }
        }


    }
}
