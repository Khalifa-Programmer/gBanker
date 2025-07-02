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
    public interface ICumMISRepository : IRepository<CumMi>
    {
        IEnumerable<Proc_Get_CUMMIS_Result> GetCumMISInfo(int? OfficeId,  string filterColumnName, string filterValue);
    }
    public class CumMISRepository : RepositoryBaseCodeFirst<CumMi>, ICumMISRepository
    {
        public CumMISRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<Proc_Get_CUMMIS_Result> GetCumMISInfo(int? OfficeId,  string filterColumnName, string filterValue)
        {
           
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
           
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<Proc_Get_CUMMIS_Result>("Proc_Get_CUMMIS @OfficeID,@filterColumnName,@filterValue", officeIdParameter, filColName, filColvalue);

        }
    }
}
