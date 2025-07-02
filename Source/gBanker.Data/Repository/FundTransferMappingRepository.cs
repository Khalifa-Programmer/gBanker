using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gBanker.Data.Repository
{
   
    public interface IFundTransferMappingRepository : IRepository<FundTransferMapping>
    {
    }
    public class FundTransferMappingRepository : RepositoryBaseCodeFirst<FundTransferMapping>, IFundTransferMappingRepository
    {
          
        public FundTransferMappingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        
        }
      
    }
}
