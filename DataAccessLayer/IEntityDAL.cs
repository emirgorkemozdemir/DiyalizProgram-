using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace DataAccessLayer
{
    public interface IEntityDal <T> where T : class ,IDataBaseEntity , new()
    {
        Task<bool> Add(T temp_entity);
        Task<bool> Delete(T temp_entity);
        Task<bool> Edit(T temp_entity);
        Task<List<T>> List(params Expression<Func<T, object>>[] includes);

        Task<T> GetbyId(int id);
    }
}
