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
    public interface ISavingCorrectionRepository : IRepository<SavingCorrectionTrx>
    {
        IEnumerable<getSavingsCorrection_Result> GetSavingCorrectionDetail(int? OrgID, int? officeID, DateTime? collectionDate);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? officeID, string vday);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate);
        int savingCorrection(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? NoAccount, int? transType, Nullable<decimal> SavingInstallment, Nullable<decimal> withdrawal, Nullable<decimal> Penalty, DateTime? Transdate);
    }
    public class SavingCorrectionRepository : RepositoryBaseCodeFirst<SavingCorrectionTrx>, ISavingCorrectionRepository
    {
        public SavingCorrectionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? officeID, string vday)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            return DataContext.Database.SqlQuery<proc_get_SpecialSavingCollection_Result>("proc_get_SpecialSavingCollection @officeId,@CollectionDay", officeIdParameter, vcolday);

        }


        public int delVoucher(int? officeID, DateTime? businessDate)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var date = new SqlParameter("@BusinessDate", businessDate);
            return DataContext.Database.ExecuteSqlCommand("delVoucher @OfficeID,@BusinessDate", officeIdParameter, date);
        }








        public int savingCorrection(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? NoAccount, int? transType, decimal? SavingInstallment, decimal? withdrawal, decimal? Penalty, DateTime? Transdate)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);

            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var NoAccountParameter = new SqlParameter("@NoAccount", NoAccount);
            var transTypeParameter = new SqlParameter("@TransType", transType);
            var SavingInstallmentParameter = new SqlParameter("@SavingInstallment", SavingInstallment);
            var withdrawalParameter = new SqlParameter("@withdrawal", withdrawal);
            var PenaltyParameter = new SqlParameter("@Penalty", Penalty);
            var TransdateParameter = new SqlParameter("@Transdate", Transdate);




            return DataContext.Database.ExecuteSqlCommand("SavingsCorrection @OrgID,@OfficeID,@CenterID,@MemberID,@ProductID,@NoAccount,@TransType,@SavingInstallment,@withdrawal,@Penalty,@Transdate", orgIdParameter, officeIdParameter, centerIDParameter, memberIDParameter, productIDParameter, NoAccountParameter, transTypeParameter, SavingInstallmentParameter, withdrawalParameter, PenaltyParameter, TransdateParameter);


        }

        public IEnumerable<getSavingsCorrection_Result> GetSavingCorrectionDetail(int? OrgID, int? officeID, DateTime? collectionDate)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var collectionDateParameter = new SqlParameter("@TransDate", collectionDate);
            return DataContext.Database.SqlQuery<getSavingsCorrection_Result>("getSavingsCorrection @OrgID,@officeId,@TransDate", orgIdParameter,officeIdParameter, collectionDateParameter);

        }
    }
}
