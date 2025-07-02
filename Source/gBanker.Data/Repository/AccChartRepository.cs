using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gBanker.Data.Repository
{
    public interface IAccChartRepository : IRepository<AccChart>
    {
        IEnumerable<DBAccChartDetailModel> GetAccChartDetail(int? orgId, string filterColumnName, string filterValue, out long TotCount);
    }
    public class AccChartRepository : RepositoryBaseCodeFirst<AccChart>, IAccChartRepository
    {
        public AccChartRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBAccChartDetailModel> GetAccChartDetail(int? orgId, string filterColumnName, string filterValue, out long TotCount)
        {
            IQueryable<AccChart> results = null;
            if (filterColumnName == "AccCode")
                results = DataContext.AccCharts.Where(w => w.OrgID == orgId && w.IsActive == true && w.AccCode.Contains(filterValue)).OrderBy(w => w.AccCode);
            else if (filterColumnName == "AccName")
                results = DataContext.AccCharts.Where(w => w.OrgID == orgId && w.IsActive == true && w.AccName.Contains(filterValue)).OrderBy(w => w.AccCode);
            else
                results = DataContext.AccCharts.Where(w => w.OrgID == orgId && w.IsActive == true).OrderBy(w => w.AccCode);
            TotCount = results.LongCount();            
            
            var obj = results.Select(s => new DBAccChartDetailModel()
            {
                AccID = s.AccID,
                AccCode = s.AccCode,
                AccName = s.AccName,
                AccLevel = s.AccLevel,
                FirstLevel = s.FirstLevel,
                SecondLevel = s.SecondLevel,
                ThirdLevel = s.ThirdLevel,
                FourthLevel= s.FourthLevel,
                FifthLevel = s.FifthLevel,
                CategoryID = s.CategoryID,
                CategoryName = s.AccCategory.CategoryName,
                OfficeLevel = s.OfficeLevel,
                IsTransaction = s.IsTransaction,
                IsActive = s.IsActive,
                Nature = s.Nature,
                ModuleID = s.ModuleID,
                NoteID = s.NoteID,
                NoteName = s.AccNote.NoteNo + ", " + s.AccNote.NoteName              
            });

            return obj;
        }
    }
}
