using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using System;
using System.Linq;

namespace gBanker.Data.Repository
{
    public interface IAccTrxDetailRepository : IRepository<AccTrxDetail>
    {
        IEnumerable<AccTrxDetail> GetByTrxMasterId(long id);
    }

    public class AccTrxDetailRepository : RepositoryBaseCodeFirst<AccTrxDetail>, IAccTrxDetailRepository
    {
        public AccTrxDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<AccTrxDetail> GetByTrxMasterId(long id)
        {
            var lst = DataContext.AccTrxDetails.Join(DataContext.AccCharts, acc => acc.AccID, crt => crt.AccID,
          (acc, crt) => new { acc, crt }).Where(w => w.acc.IsActive == true && w.acc.TrxMasterID == id).Select(a => new AccTrxDetail() { Narration = a.acc.Narration, Debit = a.acc.Debit, TrxDetailsID = a.acc.TrxDetailsID, AccChart = new AccChart() { AccName = a.crt.AccName } });
            return lst;
        }
    }
}
