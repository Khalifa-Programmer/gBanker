using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels;
using gBanker.Service.SMSSenderServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface ISMSSenderService
    {
        Task<IEnumerable<SentLogSMSSummaryModel>> GetSentLogSMSSummaryByFilter(BaseSearchFilter filter);
        SMSSendResponse SendSMS(SMSSendRequest request);
    }   
}
