using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IDailyLoanTrxRepository : IRepository<DailyLoanTrx>
    {
        IEnumerable<DailyLoanTrx> GetDailyLoanTrxDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID,int?OfficeID);
        DailyLoanTrx IsDataExistByDate(DateTime date);
    }
    public class DailyLoanTrxRepository : RepositoryBaseCodeFirst<DailyLoanTrx>, IDailyLoanTrxRepository
    {
        public DailyLoanTrxRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DailyLoanTrx> GetDailyLoanTrxDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID,int? OfficeID)
        {
            IQueryable<DailyLoanTrx> results = null;
            if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.DailyLoanTrxes.Where(p => p.IsActive == true && p.TrxType==41 && p.OfficeID==OfficeID && p.MemberCode.Contains(filterValue)).OrderBy(p => p.MemberCode);
                //results = DataContext.DailyLoanTrxes.Where(p => p.IsActive == true && p.MemberCode.Contains(filterValue) && p.OrgID == OrgID).OrderBy(p => p.MemberCode);

            else

                results = DataContext.DailyLoanTrxes.Where(p => p.IsActive == true && p.TrxType == 41 && p.OfficeID == OfficeID).OrderBy(p => p.MemberCode);
                //results = DataContext.DailyLoanTrxes.Where(p => p.IsActive == true && p.OrgID == OrgID).OrderBy(p => p.MemberCode);

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.MemberCode).Skip(startRowIndex).Take(pageSize);
            return obj;
        }
        public DailyLoanTrx IsDataExistByDate(DateTime date)
        {
            var obj = DataContext.DailyLoanTrxes.Where(f=>f.TrxDate == date).Take(1).FirstOrDefault();
            return obj;
        }
    }
}


