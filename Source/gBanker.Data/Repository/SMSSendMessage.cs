using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ISMSSendMessageRepository 
    {
        IEnumerable<SMSMobileNoInfoModel> GetSMSMobileNumbers(string groupId);
    }
    public class SMSSendMessageRepository: ISMSSendMessageRepository
    {
        public IEnumerable<SMSMobileNoInfoModel> GetSMSMobileNumbers(string groupId)
        {
            var listing = new List<SMSMobileNoInfoModel>();
            var sqlCommand = $@"[dbo].[SMS_GET_MobileNo] {groupId}";

            using (var db = new gBankerDbContext())
            {
                listing = db.Database
                                .SqlQuery<SMSMobileNoInfoModel>(sqlCommand)
                                .ToList();
            }
            return listing;
        }
    }
}
