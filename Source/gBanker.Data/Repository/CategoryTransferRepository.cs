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
    public interface ICategoryTransferRepository : IRepository<TransferHistory>
    {
        IEnumerable<DBCategoryTransferDetails> GetCategoryTransferDetail(int? officeID);
        IEnumerable<getTransferHistory_Result> GetTransferDetail(int OrgID, int? officeID);
        int CateGoryTransfer(int? orgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<int> memberID, Nullable<int> memberCategoryID, Nullable<int> productID, Nullable<decimal> savBalance, Nullable<int> newMemberCategoryID, Nullable<System.DateTime> transdate, Nullable<int> newProductID);
    }

    public class CategoryTransferRepository : RepositoryBaseCodeFirst<TransferHistory>, ICategoryTransferRepository
    {
        public CategoryTransferRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


      

        public IEnumerable<getTransferHistory_Result> GetTransferDetail(int OrgID,int? officeID)

        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);

            return DataContext.Database.SqlQuery<getTransferHistory_Result>("getTransferHistory @OrgID,@officeId",orgIdParameter, officeIdParameter);
        }


        public int CateGoryTransfer(int? orgID, int? officeID, int? centerID, int? memberID, int? memberCategoryID, int? productID, decimal? savBalance, int? newMemberCategoryID, DateTime? transdate, int? newProductID)
        {
            var orgIDParameter = new SqlParameter("@orgID", orgID);
            var officeIdParameter = new SqlParameter("@OfficeId", officeID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);
            var MemberIDParameter = new SqlParameter("@MemberID", memberID);
            var MemberCategoryIDParameter = new SqlParameter("@MemberCategoryID", memberCategoryID);
            var ProductIDParameter = new SqlParameter("@ProductID", productID);
            var SavBalanceParameter = new SqlParameter("@SavBalance", savBalance);
            var NewMemberCategoryIDParameter = new SqlParameter("@NewMemberCategoryID", newMemberCategoryID);
            var TransdateParameter = new SqlParameter("@Transdate", transdate);
            var NewProductIDParameter = new SqlParameter("@NewProductID", newProductID);
            return DataContext.Database.ExecuteSqlCommand("CateGoryTransfer @orgID,@OfficeId,@CenterID,@MemberID,@MemberCategoryID,@ProductID,@SavBalance,@NewMemberCategoryID,@Transdate,@NewProductID", orgIDParameter,officeIdParameter, centerIDParameter, MemberIDParameter, MemberCategoryIDParameter, ProductIDParameter, SavBalanceParameter, NewMemberCategoryIDParameter, TransdateParameter, NewProductIDParameter);
        }

        public IEnumerable<DBCategoryTransferDetails> GetCategoryTransferDetail(int? officeID)
        {
            var obj = DataContext.TransferHistories.Where(x =>  x.OfficeID == officeID)
               .Select(s => new DBCategoryTransferDetails()
               {
                   TransferHistoryID = s.TransferHistoryID,
                   OfficeID = s.OfficeID,
                   //OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                   //OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                   //CenterCode = s.Center.CenterCode,
                   //MemberCode = s.Member.MemberCode,
                   //MemberName = s.Member.MemberCode+" - "+ s.Member.FirstName+""+s.Member.LastName+""+s.Member.LastName,
                   //ProductCode = s.Product.ProductCode,
                   MemberCategoryId=s.MemberCategoryId,
                   TrMemberCategoryID=s.TrMemberCategoryID,
                   Principal = s.Principal
                  


               }).OrderBy(x => x.CenterCode).ThenBy(x => x.MemberCode);

            return obj;
        }
    }
}
