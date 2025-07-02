using BasicDataAccess;
using BasicDataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{
    public interface IMRAReportService
    {
        DataSet GetDataMraReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMraMISReport3A<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMraMISReport3B1<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMraMISReport3B2<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMraMISReport3B3<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMraMISReport3B4<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMraMISReport4A<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMraMISReport4B1<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMraMISReport4B2<TParamOType>(TParamOType target) where TParamOType : class;
    }
    public class MRAReportService : IMRAReportService
    {
        public DataSet GetDataMraReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_RptMRA";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMraMISReport3A<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_MRA_MIS_03A_Part01";
            using (var gbData = new gBankerReportDataAccess())
           // using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMraMISReport3B1<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_MRA_MIS_03B_Part01";
            using (var gbData = new gBankerReportDataAccess())
           // using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMraMISReport3B2<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_MRA_MIS_03B_Part02";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMraMISReport3B3<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_MRA_MIS_03B_Part03";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMraMISReport3B4<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_MRA_MIS_03B_Part04";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMraMISReport4A<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_MRA_MIS_04A_Part01";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMraMISReport4B1<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_MRA_MIS_04B_Part01";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMraMISReport4B2<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_MRA_MIS_04B_Part02";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }




    }
}

