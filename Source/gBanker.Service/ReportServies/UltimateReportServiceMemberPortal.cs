using BasicDataAccess;
using BasicDataAccess.Data;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using gBankerCodeFirstMigration.Db;
using System;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{

    public interface IUltimateReportServiceMemberPortal
    {
        DataSet GetMemberPortalData<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        //DataSet GetMemberPortalData_DynamicServer<TParamOType>(TParamOType target, string storeProcedureName, string ServerIP, string User, string Password, string DatabaseName) where TParamOType : class;

    }// End Interface

    public class UltimateReportServiceMemberPortal : IUltimateReportServiceMemberPortal
    {
        private readonly ICenterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public UltimateReportServiceMemberPortal(ICenterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }


        public DataSet GetMemberPortalData<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_DailySavingsCollection";
            using (var gbData = new gBankerMemberPortalAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerMemberPortalAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        //public DataSet GetMemberPortalData_DynamicServer<TParamOType>(TParamOType target, string storeProcedureName, string ServerIP, string User, string Password, string DatabaseName) where TParamOType : class
        //{
        //    gBankerMemberPortalAccess obj = new gBankerMemberPortalAccess();
        //    var vConnection = obj.DynamicConnectionString(ServerIP, User, Password, DatabaseName);
            
        //    using (var gbData = new gBankerMemberPortalAccess())
        //    {
        //        return gbData.GetDataOnDateset(storeProcedureName, target);
        //    }
        //}


    }// END Class
}// End Namespace
