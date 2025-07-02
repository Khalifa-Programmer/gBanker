using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using System;
using System.Linq;

namespace gBanker.Data.Repository
{
    public interface IInvTrxDetailRepository : IRepository<Inv_TrxDetail>
    {
       
    }

    public class InvTrxDetailRepository : RepositoryBaseCodeFirst<Inv_TrxDetail>, IInvTrxDetailRepository
    {
        public InvTrxDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        
    }
}
