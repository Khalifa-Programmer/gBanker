using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface IServiceBase<T> where T : class
    {

        IEnumerable<T> GetAll();
        T GetById(int id);
        T Create(T objectToCreate);
        void Update(T objectToUpdate);
        void Delete(int id);
        bool Inactivate(long id, DateTime? inactiveDate);
        bool IsContinued(long id);
        void Save();
        T GetByIdLong(long id);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        
    }
}
