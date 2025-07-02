using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;

namespace gBanker.Data.Repository
{
   
    public interface IProductXEmploymentProductMappingRepository : IRepository<ProductXEmploymentProductMapping>
    {
        
    }
    public class ProductXEmploymentProductMappingRepository : RepositoryBaseCodeFirst<ProductXEmploymentProductMapping>, IProductXEmploymentProductMappingRepository
    {
          
        public ProductXEmploymentProductMappingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        
        }
  

    }
}
