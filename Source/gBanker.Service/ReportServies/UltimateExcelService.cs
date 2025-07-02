using BasicDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{

    public interface IUltimateExcelService
    {
        //  DataSet GetDataDailySavingCollectionReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;


    }
    public class UltimateExcelService : IUltimateExcelService
    {
        //public DataSet GetDataDailySavingCollectionReport<TParamOType>(TParamOType target) where TParamOType : class
        //{
        //    var storeProcedureName = "Rpt_DailySavingsCollection";
        //    using (var gbData = new gBankerExcelAccess())
        //    {
        //        return gbData.GetDataOnDateset(storeProcedureName, target);
        //    }
        //}

        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerExcelDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }//



    }// END Class
}// END NameSpace
