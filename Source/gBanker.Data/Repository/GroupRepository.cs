using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace gBanker.Data.Repository
{
    public interface IGroupRepository : IRepository<Group>
    {
        IEnumerable<DBGroupDeatil> GetGroupDetail(int OrgID,int? officeID);
    }
    public class GroupRepository : RepositoryBaseCodeFirst<Group>, IGroupRepository
    {
        public GroupRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


        public IEnumerable<DBGroupDeatil> GetGroupDetail(int OrgID, int? officeID)
        {
            var obj = DataContext.Groups.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID==OrgID && x.OfficeID == officeID)
                .Select(s => new DBGroupDeatil()
                {
                    GroupID = s.GroupID,
                    GroupCode = s.GroupCode,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                    FormationDate = s.FormationDate,
                    IsActive = s.IsActive,

                }).OrderBy(g => g.OfficeCode).ThenBy(g => g.GroupCode);

            return obj;
        }
    }
}
