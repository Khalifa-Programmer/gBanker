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
    public interface IMemberPassBookRegisterRepository : IRepository<MemberPassBookRegister>
    {
        IEnumerable<getPassBookRegister_Result> getPassBookRegister( int? officeID,int? orgID);
       
    }
    public class MemberPassBookRegisterRepository : RepositoryBaseCodeFirst<MemberPassBookRegister>, IMemberPassBookRegisterRepository
    {
        public MemberPassBookRegisterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<getPassBookRegister_Result> getPassBookRegister(int? officeID, int? orgID)
        {
            var orgIdParameter = new SqlParameter("@OrgID", orgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);

            return DataContext.Database.SqlQuery<getPassBookRegister_Result>("getPassBookRegister @OfficeID,@OrgID", officeIdParameter, orgIdParameter);

        }
    }
}
