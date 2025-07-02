using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace gBanker.Data.Repository
{
    public interface IAccTrxMasterRepository : IRepository<AccTrxMaster>
    {
        IEnumerable<Proc_Get_AccountDetails_Result> Proc_Get_AccountDetails(Nullable<int> orgID, Nullable<int> officeID,DateTime? TrxDate);
    }
    public class AccTrxMasterRepository : RepositoryBaseCodeFirst<AccTrxMaster>, IAccTrxMasterRepository
    {
        public AccTrxMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


        public IEnumerable<Proc_Get_AccountDetails_Result> Proc_Get_AccountDetails(Nullable<int> orgID, Nullable<int> officeID, DateTime? TrxDate)
        {
            var orgIDParameter = new SqlParameter("@orgID", orgID);
            var officeIdParameter = new SqlParameter("@OfficeId", officeID);
            var TrxDateParameter = new SqlParameter("@TrxDate", TrxDate);




            return DataContext.Database.SqlQuery<Proc_Get_AccountDetails_Result>("Proc_Get_AccountDetails @orgID,@OfficeId,@TrxDate", orgIDParameter, officeIdParameter,TrxDateParameter);

        }
    }
}
