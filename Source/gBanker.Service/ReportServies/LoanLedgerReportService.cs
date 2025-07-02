using BasicDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{
    public interface ILoanLedgerReportService
    {
        DataSet GetDataLoanLedgerInfo<TParamOType>(TParamOType target) where TParamOType : class;
    }

    public class LoanLedgerReportService : ILoanLedgerReportService
    {
        public DataSet GetDataLoanLedgerInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_LoanLedger";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
    }
}
