using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration;
using System.Data.SqlClient;

namespace gBanker.Data.Repository
{
    public interface IMemberNomineeRepository : IRepository<MemberNominee>
    {
        //int SetOpeningSavingEntry(int? orgID, int? officeID);
    }
    public class MemberNomineeRepository : RepositoryBaseCodeFirst<MemberNominee>, IMemberNomineeRepository
    {
        public MemberNomineeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        //public int SetOpeningSavingEntry(int? orgID, int? officeID)
        //{
        //    var orgIdParameter = new SqlParameter("@OrgID", orgID);
        //    var officeIdParameter = new SqlParameter("@OfficeId", officeID);

        //    return DataContext.Database.ExecuteSqlCommand("WriteOffHistoryInsertToLedger @OrgID,@OfficeId", orgIdParameter, officeIdParameter);

        //}
    }
}
