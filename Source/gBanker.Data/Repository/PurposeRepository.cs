using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public class PurposeRepository : RepositoryBaseCodeFirst<Purpose>, IPurposeRepository
    {
        public PurposeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<Purpose> GetPurposeDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount,int? OrgID)
        {
            IQueryable<Purpose> results = null;
            if (filterColumnName == "PurposeCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.Purposes.Where(p => p.IsActive == true && p.PurposeCode.Contains(filterValue) && p.OrgID==OrgID).OrderBy(p => p.PurposeCode);

            else

                results = DataContext.Purposes.Where(p => p.IsActive == true && p.OrgID == OrgID).OrderBy(p => p.PurposeCode);

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.PurposeCode).Skip(startRowIndex).Take(pageSize);
            return obj;
        }
    }
    public interface IPurposeRepository : IRepository<Purpose>
    {
        IEnumerable<Purpose> GetPurposeDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount,int? OrgID);
    }
}

