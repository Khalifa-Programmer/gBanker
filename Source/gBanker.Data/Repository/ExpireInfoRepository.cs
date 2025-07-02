using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IExpireInfoRepository : IRepository<ExpireInfo>
    {
        int setExpireInfo(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, string expiryName, string relation, Nullable<System.DateTime> expireDate, string remarks, Nullable<int> orgID, string createUser, Nullable<System.DateTime> createDate, Nullable<int> ExpireStatus,long? cLoanSummaryID, string ApplicantName, string MobileNumber);
        IEnumerable<getExpireInfo_Result> getExpireInfo(Nullable<int> officeId, string filterColumnName, string filterValue);
    }
    public class ExpireInfoRepository : RepositoryBaseCodeFirst<ExpireInfo>, IExpireInfoRepository
    {
        public ExpireInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<getExpireInfo_Result> getExpireInfo(int? officeId, string filterColumnName, string filterValue)
        {
            var officeIdParameter = new SqlParameter("@OfficeId", officeId);

            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<getExpireInfo_Result>("getExpireInfo @OfficeId,@filterColumnName,@filterValue", officeIdParameter,filColName, filColvalue);


        }

        public int setExpireInfo(int? officeID, int? centerID, long? memberID, string expiryName, string relation
            , DateTime? expireDate, string remarks, int? orgID
            , string createUser, DateTime? createDate,int? ExpireStatus,long? cLoanSummaryID, string ApplicantName, string MobileNumber)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var expiryNameParameter = new SqlParameter("@ExpiryName", expiryName);
            var relationParameter = new SqlParameter("@Relation", relation);
            var expireDateParameter = new SqlParameter("@ExpireDate", expireDate);
            var remarksParameter = new SqlParameter("@Remarks", remarks);
            var orgIDParameter = new SqlParameter("@OrgID", orgID);
            var CreateUserParameter = new SqlParameter("@CreateUser", createUser);
            var CreateDateParameter = new SqlParameter("@CreateDate", createDate);
            var ExpireStatuseParameter = new SqlParameter("@ExpireStatus", ExpireStatus);
            var cLoanSummaryIDParameter = new SqlParameter();
            if (cLoanSummaryID.HasValue)
             cLoanSummaryIDParameter = new SqlParameter("@cLoanSummaryID", cLoanSummaryID.Value);
            else
                cLoanSummaryIDParameter = new SqlParameter("@cLoanSummaryID", DBNull.Value);

            var applicantName = new SqlParameter("@ApplicantName", ApplicantName);
            var mobileNumber = new SqlParameter("@MobileNumber", MobileNumber);

            return DataContext.Database.ExecuteSqlCommand("setExpireInfoBuro @OfficeID,@CenterID,@MemberID,@ExpiryName,@Relation,@ExpireDate,@Remarks,@OrgID,@CreateUser,@CreateDate,@ExpireStatus,@cLoanSummaryID,@ApplicantName,@MobileNumber"
                , officeIdParameter, centerIDParameter, memberIDParameter, expiryNameParameter, relationParameter, expireDateParameter
                ,remarksParameter,orgIDParameter,CreateUserParameter,CreateDateParameter,ExpireStatuseParameter, cLoanSummaryIDParameter,applicantName,mobileNumber);
        }
    }
}
