using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
//using gBanker.Data.Db;
using System.Collections.Generic;
namespace gBanker.Data.Repository
{
    public interface IGeoLocationRepository : IRepository<GeoLocation>
    {
    }
    public class GeoLocationRepository : RepositoryBaseCodeFirst<GeoLocation>, IGeoLocationRepository
    {
        public GeoLocationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
