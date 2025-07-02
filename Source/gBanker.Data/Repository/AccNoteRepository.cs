using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;


namespace gBanker.Data.Repository
{
    public interface IAccNoteRepository : IRepository<AccNote>
    {
    }
    public class AccNoteRepository : RepositoryBaseCodeFirst<AccNote>, IAccNoteRepository
    {
        public AccNoteRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
