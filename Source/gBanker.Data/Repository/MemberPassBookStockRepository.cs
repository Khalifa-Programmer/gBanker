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
    public interface IMemberPassBookStockRepository :IRepository<MemberPassBookStock>
    {

        IEnumerable<getMemberPassBookStock_Result> getPassBookStock(int? officeID);

    }
    public class MemberPassBookStockRepository : RepositoryBaseCodeFirst<MemberPassBookStock>, IMemberPassBookStockRepository
    {
        public MemberPassBookStockRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<getMemberPassBookStock_Result> getPassBookStock(int? officeID)
        {
           
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);

            return DataContext.Database.SqlQuery<getMemberPassBookStock_Result>("getMemberPassBookStock @OfficeID", officeIdParameter);
        }
    }
}
