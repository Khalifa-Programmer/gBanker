using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public class MemberCategoryRepository : RepositoryBaseCodeFirst<MemberCategory>, IMemberCategoryRepository
    {

        public MemberCategoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }

    public interface IMemberCategoryRepository : IRepository<MemberCategory>
    {

    }
}
