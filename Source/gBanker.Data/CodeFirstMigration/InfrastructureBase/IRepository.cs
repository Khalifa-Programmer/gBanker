using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration.InfrastructureBase
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IQueryable<T> GetAllQueryable();
        List<TResult> GetSqlResult<TResult, TParam>(string sql, TParam param)where TResult:class;

        #region Async
        //Task<IEnumerable<T>> GetAllAsync();
        //Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);
        //Task<T> GetAsync(Expression<Func<T, bool>> where);
        #endregion Async
        // IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order);
    }
}
