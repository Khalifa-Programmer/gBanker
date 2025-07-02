using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels;

namespace gBanker.Service
{
    public interface IOLRSAccChartMappingService : IServiceBase<OLRSAccChartMapping>
    {
        Task<IEnumerable<AccChart>> GetAccChartListByLevel(AccChartSearchFilter filter);
        Task<IEnumerable<AccChart>> GetAccChartList(AccChartSearchFilter filter);
        Task<IEnumerable<PO_A_ACC_HEADModel>> GetPOAccChartList();
        Task<IEnumerable<OLRSAccChartMapping>> GetOLRSAccChartMappingList();
        Task<bool> AddAccChartMapping(OLRSAccChartMapping accChartMapping);
        bool CheckAccChartMappingDuplicacy(OLRSAccChartMapping param);
    } 
    public class OLRSAccChartMappingService : IOLRSAccChartMappingService
    {
        private readonly IOLRSAccChartMappingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OLRSAccChartMappingService(IOLRSAccChartMappingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #region Public Methods


        public async Task<IEnumerable<AccChart>> GetAccChartListByLevel(AccChartSearchFilter filter)
        {
            var listing = new List<AccChart>();
            try
            {
                return await repository.GetAccChartListByLevel(filter);
            }
            catch (Exception ex)
            {
                return new List<AccChart>();
            }
        }

        public async Task<IEnumerable<AccChart>> GetAccChartList(AccChartSearchFilter filter)
        {           
            try
            {
                return await repository.GetAccChartList(filter);
            }
            catch (Exception ex)
            {
                return new List<AccChart>();
            }
        }

        public async Task<IEnumerable<PO_A_ACC_HEADModel>> GetPOAccChartList()
        {
            var listing = new List<PO_A_ACC_HEADModel>();
            try
            {               
                return await repository.GetPOAccChartList();
            }
            catch (Exception ex)
            {
                return new List<PO_A_ACC_HEADModel>();
            }
        }

        public async Task<IEnumerable<OLRSAccChartMapping>> GetOLRSAccChartMappingList()
        {
            var listing = new List<OLRSAccChartMapping>();
            try
            {
                return await repository.GetOLRSAccChartMappingList();
            }
            catch (Exception ex)
            {
                return new List<OLRSAccChartMapping>();
            }
        }        
        public async Task<bool> AddAccChartMapping(OLRSAccChartMapping accChartMapping)
        {
            try
            {
                await repository.AddAccChartMapping(accChartMapping);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CheckAccChartMappingDuplicacy(OLRSAccChartMapping param)
        {
            try
            {
                var result = repository.CheckAccChartMappingDuplicacy(param);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Auto Methods

        public OLRSAccChartMapping Create(OLRSAccChartMapping objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public IEnumerable<OLRSAccChartMapping> GetAll()
        {
            throw new NotImplementedException();
        }

        public OLRSAccChartMapping GetById(int id)
        {
            throw new NotImplementedException();
        }

        public OLRSAccChartMapping GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OLRSAccChartMapping> GetMany(Expression<Func<OLRSAccChartMapping, bool>> where)
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
            unitOfWork.Commit();
        }

        public void Update(OLRSAccChartMapping objectToUpdate)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
