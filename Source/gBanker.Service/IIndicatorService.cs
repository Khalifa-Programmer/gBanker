using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;

namespace gBanker.Service
{
    public interface IIndicatorService : IServiceBase<Indicator>
    {
        Task<IEnumerable<Indicator>> GetIndicators();
        Task<IEnumerable<POIndicatorRelatedAccCodeModel>> GetPOAccCodes(BaseSearchFilter filter);
        Task<bool> UpdateIndicators(List<Indicator> indicators);

        Task<bool> AddManualProgramData(ProgramDataManualDataModel model);
        Task<bool> AddFinancialData(FinancialDataModel model);
        Task<bool> AddBasicData(BasicDataModel model);
        Task<IEnumerable<Indicator>> GetIndicatorList(BaseSearchFilter filter);
        Task<bool> AddIndicator(Indicator indicator);
        Task<IEnumerable<Indicator>> GetIndicatorListByIsManual(IndicatorSearchFilter filter);
        Task<IEnumerable<Indicator>> GetIndicatorsByFD();
        Task<IEnumerable<POIndicatorRelatedAccCodeModel>> GetPOAccCodesByFilter(BaseSearchFilter filter);
    }
    public class IndicatorService : IIndicatorService
    {
        private readonly IIndicatorRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public IndicatorService(IIndicatorRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #region Public Methods

        public async Task<IEnumerable<Indicator>> GetIndicators()
        {
            try
            {
                return await repository.GetIndicators();
            }
            catch
            {
                return new List<Indicator>();
            }

        }
        public async Task<IEnumerable<Indicator>> GetIndicatorsByFD()
        {
            try
            {
                return await repository.GetIndicatorsByFD();
            }
            catch
            {
                return new List<Indicator>();
            }

        }

        public async Task<IEnumerable<POIndicatorRelatedAccCodeModel>> GetPOAccCodes(BaseSearchFilter filter)
        {
            var listing = new List<POIndicatorRelatedAccCodeModel>();
            try
            {
                return await repository.GetPOAccCodes(filter);
            }
            catch
            {
                return new List<POIndicatorRelatedAccCodeModel>();
            }
        }
        public async Task<IEnumerable<POIndicatorRelatedAccCodeModel>> GetPOAccCodesByFilter(BaseSearchFilter filter)
        {
            var listing = new List<POIndicatorRelatedAccCodeModel>();
            try
            {
                return await repository.GetPOAccCodesByFilter(filter);
            }
            catch
            {
                return new List<POIndicatorRelatedAccCodeModel>();
            }
        }
        public async Task<bool> UpdateIndicators(List<Indicator> indicators)
        {
            try
            {
                return await repository.UpdateIndicators(indicators);
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Indicator>> GetIndicatorList(BaseSearchFilter filter)
        {
            var listing = new List<Indicator>();
            try
            {                
                return await repository.GetIndicatorList(filter);
            }
            catch (Exception ex)
            {
                return new List<Indicator>();
            }
        }
        public async Task<IEnumerable<Indicator>> GetIndicatorListByIsManual(IndicatorSearchFilter filter)
        {
            var listing = new List<Indicator>();
            try
            {
                return await repository.GetIndicatorListByIsManual(filter);
            }
            catch (Exception ex)
            {
                return new List<Indicator>();
            }
        }


        public async Task<bool> AddManualProgramData(ProgramDataManualDataModel model)
        {
            try
            {
                var isAdded = await repository.AddManualProgramData(model);
                return isAdded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddFinancialData(FinancialDataModel model)
        {
            try
            {
                var isAdded = await repository.AddFinancialData(model);
                return isAdded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddBasicData(BasicDataModel model)
        {
            try
            {
                var isAdded = await repository.AddBasicData(model);
                return isAdded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddIndicator(Indicator indicator)
        {
            try
            {                
                return await repository.AddIndicator(indicator);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Auto Methods

        public Indicator Create(Indicator objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Indicator> GetAll()
        {
            throw new NotImplementedException();
        }

        public Indicator GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Indicator GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Indicator> GetMany(Expression<Func<Indicator, bool>> where)
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

        public void Update(Indicator objectToUpdate)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
