using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IFamilyMemberSameHouseholdRepository : IRepository<FamilyMemberSameHousehold>
    {
    }
    public class FamilyMemberSameHouseholdRepository : RepositoryBaseCodeFirst<FamilyMemberSameHousehold>, IFamilyMemberSameHouseholdRepository
    {
        public FamilyMemberSameHouseholdRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
