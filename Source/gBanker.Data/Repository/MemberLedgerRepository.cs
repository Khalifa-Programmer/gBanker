using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IMemberLedgerRepository : IRepository<getGetLoanLedgerMemberWise_Result>
    {
        IEnumerable<getGetLoanLedgerMemberWise_Result> getGetLoanLedgerMemberWise(int OrgID,int? OfficeId, int? Memberid);

    }
    public class MemberLedgerRepository : RepositoryBaseCodeFirst<getGetLoanLedgerMemberWise_Result>, IMemberLedgerRepository
    {
        public MemberLedgerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


        public IEnumerable<getGetLoanLedgerMemberWise_Result> getGetLoanLedgerMemberWise(int OrgID, int? OfficeId, int? Memberid)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeId", OfficeId);
            var memberIdParameter = new SqlParameter("@memberId", Memberid);
            return DataContext.Database.SqlQuery<getGetLoanLedgerMemberWise_Result>("getGetLoanLedgerMemberWise @OrgID, @OfficeId,@memberId", orgIdParameter, officeIdParameter, memberIdParameter);
            
        }
    }
}
