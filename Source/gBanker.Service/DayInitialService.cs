using BasicDataAccess;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IDayInitialService : IServiceBase<Prcs_DayInitial_Result>
    {

        int DayInitialProcess(int? OfficeId, DateTime? vDate);
        int DayInitialProcess(int? OfficeId, DateTime? vDate, string CreateUser, DateTime? CreateDate, int OrgID);
        string ValidateDayInitial(int? OfficeId, out DateTime? Transactiondate, out string OrganizationName, out string Processtype, out DateTime? LastDayEndDate, int OrgID);
    }
    public class DayInitialService : IDayInitialService
    {
        private readonly IDayInitialRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public DayInitialService(IDayInitialRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }

        public int DayInitialProcess(int? OfficeId, DateTime? vDate)
        {
            return repository.DayInitialProcess(OfficeId, vDate);
        }

        public IEnumerable<Prcs_DayInitial_Result> GetAll()
        {
            throw new NotImplementedException();
        }

        public Prcs_DayInitial_Result GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Prcs_DayInitial_Result Create(Prcs_DayInitial_Result objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(Prcs_DayInitial_Result objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }


        public string ValidateDayInitial(int? OfficeId, out DateTime? Transactiondate, out string OrganizationName, out string Processtype, out DateTime? LastDayEndDate, int OrgID)
        {
            
            var storeProcedureName = "validateDayIntial";
            var obj = new { OfficeId = OfficeId, OrgID = OrgID };
            string transactionDay = string.Empty;
            Transactiondate = default(DateTime?);
            OrganizationName = string.Empty;
            Processtype = string.Empty;
            LastDayEndDate = default(DateTime?);
            using (var gbData = new gBankerDataAccess())
            {
                var ds = gbData.GetDataOnDateset(storeProcedureName, obj);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    transactionDay = ds.Tables[0].Rows[0]["WeekDayName"].ToString();
                    var trDate = ds.Tables[0].Rows[0]["vBusinessDate"].ToString();
                    if (!string.IsNullOrEmpty(trDate))
                        Transactiondate = DateTime.Parse(trDate);
                    OrganizationName = ds.Tables[0].Rows[0]["OrganizationName"].ToString();
                    Processtype = ds.Tables[0].Rows[0]["ProcessType"].ToString();
                    trDate = ds.Tables[0].Rows[0]["LastDayEndDate"].ToString();
                    if (!string.IsNullOrEmpty(trDate))
                        LastDayEndDate = DateTime.Parse(trDate);

                }
            }

            return transactionDay;
        }


        public int DayInitialProcess(int? OfficeId, DateTime? vDate, string CreateUser, DateTime? CreateDate, int OrgID)
        {
            return repository.DayInitialProcess(OfficeId, vDate, CreateUser, CreateDate,OrgID);
        }


        public Prcs_DayInitial_Result GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Prcs_DayInitial_Result> GetMany(Expression<Func<Prcs_DayInitial_Result, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
