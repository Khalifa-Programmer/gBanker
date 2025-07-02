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
    public interface IMiscellaneouRepository : IRepository<Miscellaneou>
    {
        IEnumerable<Proc_get_Miscellaneou_Result> GetMiscellaneou(int? officeID, DateTime? TrxDate);
    }
    public class MiscellaneouRepository : RepositoryBaseCodeFirst<Miscellaneou>, IMiscellaneouRepository
    {
        public MiscellaneouRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<Proc_get_Miscellaneou_Result> GetMiscellaneou(int? officeID, DateTime? TrxDate)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var date = new SqlParameter("@TrxDate", TrxDate);
            return DataContext.Database.SqlQuery<Proc_get_Miscellaneou_Result>("Proc_get_Miscellaneou @OfficeID,@TrxDate", officeIdParameter, date);

        }
    }
}
