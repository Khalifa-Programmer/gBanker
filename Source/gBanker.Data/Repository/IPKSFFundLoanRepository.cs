using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IPKSFFundLoanRepository : IRepository<PKSFFundLoan>
    {
    }
    public class PKSFFundLoanRepository : RepositoryBaseCodeFirst<PKSFFundLoan>, IPKSFFundLoanRepository
    {
        public PKSFFundLoanRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
