using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class InvestorRepository : RepositoryBaseCodeFirst<Investor>, IInvestorRepository
    {
        public InvestorRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInvestorRepository : IRepository<Investor>
    { 
    }
}
