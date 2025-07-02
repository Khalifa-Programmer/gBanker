using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IMemberLastCodeRepository : IRepository<MemberLastCode>
    {        
    }
    public class MemberLastCodeRepository : RepositoryBaseCodeFirst<MemberLastCode>, IMemberLastCodeRepository
    {
        public MemberLastCodeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
