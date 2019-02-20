using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace odec.Entity.DAL.Interop
{
    [Obsolete("It is obsolete interface refactor on IActivatable Entity And IEntityOperations")]
    public interface IModelOperations<TEntity,TKey>  
    {

        IEnumerable<TEntity> GetTableData(bool all = false);
        Task<IEnumerable<TEntity>> GetTableDataAsync(bool all = false);
        TEntity Save(TEntity entity);

        TEntity Save(TEntity entity, out TKey key);
        KeyValuePair<TKey, TEntity> SaveReturnId(TEntity entity);
        bool Delete(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<TEntity> SaveAsync(TEntity entity);
        Task<KeyValuePair<TKey, TEntity>> SaveReturnIdAsync(TEntity entity);
    }

}
