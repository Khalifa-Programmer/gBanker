
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IFamilyGraceRepository : IRepository<FamilyGrace>
    {

        IEnumerable<getFamilyGrace_Result> GetFamilyGraceDetail(int? orgID, int? officeID);
    }
    public class FamilyGraceRepository : RepositoryBaseCodeFirst<FamilyGrace>, IFamilyGraceRepository
    {
        public FamilyGraceRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


        public IEnumerable<getFamilyGrace_Result> GetFamilyGraceDetail(int? orgID, int? officeID)
        {
            var OrgIdParameter = new SqlParameter("@OrgID", orgID);
            var officeIdParameter = new SqlParameter("@officeId", officeID);

            return DataContext.Database.SqlQuery<getFamilyGrace_Result>("getFamilyGrace @OrgID,@officeId", OrgIdParameter,officeIdParameter);

        }
    }
}
