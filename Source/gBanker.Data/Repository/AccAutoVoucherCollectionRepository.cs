
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
    public interface IAccAutoVoucherCollectionRepository : IRepository<AutoVoucherCollectionResult>
    {
        int AccAutoVoucherCollectionProcess(int? OfficeId, DateTime? vDate,int? OrgID);

    }
    public class AccAutoVoucherCollectionRepository : RepositoryBaseCodeFirst<AutoVoucherCollectionResult>, IAccAutoVoucherCollectionRepository
    {
        public AccAutoVoucherCollectionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


        public int AccAutoVoucherCollectionProcess( int? OfficeId, DateTime? vDate,int? OrgID)
        {
            
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
            var dateParameter = new SqlParameter("@BusinessDate", vDate);
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.ExecuteSqlCommand("AccAutoVoucherCollection @OfficeID,@BusinessDate,@OrgID", officeIdParameter, dateParameter,orgIdParameter);
          
        }
    }
}
