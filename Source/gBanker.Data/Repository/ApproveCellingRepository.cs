using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IApproveCellingRepository : IRepository<ApproveCelling>
    {
        IEnumerable<ApproveCelling> GetApproveCellingDetail();
        ApproveCelling GetApproveCellingbyroleAndproductId(int roleID, int? productId);


    }
    public class ApproveCellingRepository : RepositoryBaseCodeFirst<ApproveCelling>, IApproveCellingRepository
    {
        public ApproveCellingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
      
        public ApproveCelling GetApproveCellingbyroleAndproductId(int roleID, int? productId)
        {
            try
            {

                var approveCelling = DataContext.ApproveCellings
                    .Where(x => x.RoleID == roleID && x.ProductId == productId)
                    .FirstOrDefault();

                return approveCelling;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ApproveCelling> GetApproveCellingDetail()
        {
            var obj = DataContext.ApproveCellings
            .Select(s => new ApproveCelling()
            {

                ApproveCellingID = s.ApproveCellingID,
               

                RoleID = s.RoleID,
                MaxRange = s.MaxRange,
                MinRange = s.MinRange,
                RoleName = s.RoleName,
                ProdType = s.ProdType
               

            });

            return obj;
        }

      
    }
}
