using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Service
{
    public interface ISMSSendMessageService
    {
        IEnumerable<SMSMobileNoInfoModel> GetSMSMobileNumbers(string groupId);
    }
    public class SMSSendMessageService : ISMSSendMessageService
    {
        private readonly ISMSSendMessageRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SMSSendMessageService(ISMSSendMessageRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<SMSMobileNoInfoModel> GetSMSMobileNumbers(string groupId)
        {
            var listing = repository.GetSMSMobileNumbers(groupId);

            return listing;
        }
    }
}
