using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ITransactionTypeRepository : IRepository<TransactionType>
    {

    }

    public class TransactionTypeRepository : RepositoryBaseCodeFirst<TransactionType>, ITransactionTypeRepository
    {
        public TransactionTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
