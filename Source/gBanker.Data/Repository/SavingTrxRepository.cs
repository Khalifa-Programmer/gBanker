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
    public interface ISavingTrxRepository : IRepository<SavingTrx>
    {
        IEnumerable<SavingTrx> GetSavingTrx(int CenterID, long MemberID, int ProductID, int LoanTerm, string Option);
        IEnumerable<SavingTrx> GetSavingImatureList(int CenterID, long MemberID, int ProductID, int LoanTerm, string Option, int SaveYesNO);


    }
    public class SavingTrxRepository: RepositoryBaseCodeFirst<SavingTrx>, ISavingTrxRepository
    {
        public SavingTrxRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<SavingTrx> GetSavingTrx(int CenterID, long MemberID, int ProductID, int NoOfAccount, string Option)
        {
            var centerIDParameter = new SqlParameter("@CenterID", CenterID);
            var memberIDParameter = new SqlParameter("@MemberID", MemberID);
            var productIDParameter = new SqlParameter("@ProductID", ProductID);
            var noOfAccountParameter = new SqlParameter("@NoOfAccount", NoOfAccount);

            return DataContext.Database.SqlQuery<SavingTrx>("Proc_GetSavingTrx  @CenterID, @MemberID, @ProductID, @NoOfAccount", centerIDParameter, memberIDParameter, productIDParameter, noOfAccountParameter);
        }
        public IEnumerable<SavingTrx> GetSavingImatureList(int CenterID, long MemberID, int ProductID, int NoOfAccount, string Option, int SaveYesNO)
        {
            var centerIDParameter = new SqlParameter("@CenterID", CenterID);
            var memberIDParameter = new SqlParameter("@MemberID", MemberID);
            var productIDParameter = new SqlParameter("@ProductID", ProductID);
            var saveYesNO = new SqlParameter("@SaveYesNO", SaveYesNO);
            var noOfAccountParameter = new SqlParameter("@NoOfAccount", NoOfAccount);

            return DataContext.Database.SqlQuery<SavingTrx>("getImmatureLTS  @CenterID, @MemberID, @ProductID, @SaveYesNO, @NoOfAccount", centerIDParameter, memberIDParameter, productIDParameter, saveYesNO, noOfAccountParameter);
        }
    }
}
