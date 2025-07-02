using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gBanker.Data.Repository
{   
    public interface IDailySavingTrxRepository : IRepository<DailySavingTrx>
    {
        IEnumerable<DailySavingTrx> GetDailySavingTrxDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID, int? OfficeID);
        DailySavingTrx IsDataExistByDate(DateTime date);
    }
   
    public class DailySavingTrxRepository : RepositoryBaseCodeFirst<DailySavingTrx>, IDailySavingTrxRepository
    {
        public DailySavingTrxRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
     public IEnumerable<DailySavingTrx> GetDailySavingTrxDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID, int? OfficeID)
     {
         IQueryable<DailySavingTrx> results = null;
         if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
             results = DataContext.DailySavingTrxes.Where(p => p.IsActive == 1 && p.TransType == 41 && p.OfficeID==OfficeID &&p.MemberCode.Contains(filterValue)).OrderBy(p => p.MemberCode);
         
         else
             results = DataContext.DailySavingTrxes.Where(p => p.IsActive == 1 && p.TransType == 41 && p.OfficeID == OfficeID).OrderBy(p => p.MemberCode);
         
         totalCount = results.LongCount();
         var obj = results.OrderBy(x => x.MemberCode).Skip(startRowIndex).Take(pageSize);
         return obj;
     }
        public DailySavingTrx IsDataExistByDate(DateTime date)
        {
            var obj = DataContext.DailySavingTrxes.Where(f => EntityFunctions.TruncateTime(f.TransactionDate) == EntityFunctions.TruncateTime(date)).FirstOrDefault();
            return obj;
        }
    }
}

