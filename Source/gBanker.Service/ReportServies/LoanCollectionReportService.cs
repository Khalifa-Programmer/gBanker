using BasicDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{
    public interface ILoanCollectionReportService
    {
        DataSet GetDataCollectionInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataSavingCollectionInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataDisbursementInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataDisbursementInfoDisburePage<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetRepaymentInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateReceipt<TParamOType>(TParamOType target) where TParamOType : class;
    }
    public class LoanCollectionReportService : ILoanCollectionReportService
    {
        public DataSet GetDataCollectionInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getCollectionInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetDataSavingCollectionInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_DailySavingsCollection";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetDataDisbursementInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetDisburseList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetRepaymentInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getRepaymentScheduleReport";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetDataDisbursementInfoDisburePage<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetDisburseListDisbursepage";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GenerateReceipt<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetInvoice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
    }
}
