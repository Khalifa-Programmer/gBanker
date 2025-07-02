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
    public interface ICumAISRepository : IRepository<CumAI>
    {
        IEnumerable<Proc_Get_CUMAIS_Result> GetCumAISInfo(int? OfficeId, string filterColumnName, string filterValue);
    }
    public class CumAISRepository : RepositoryBaseCodeFirst<CumAI>, ICumAISRepository
    {
        public CumAISRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<Proc_Get_CUMAIS_Result> GetCumAISInfo(int? OfficeId, string filterColumnName, string filterValue)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);

            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<Proc_Get_CUMAIS_Result>("Proc_Get_CUMAIS @OfficeID,@filterColumnName,@filterValue", officeIdParameter, filColName, filColvalue);

        }
    }
}
