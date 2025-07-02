using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IPO_INFORepository : IRepository<PO_INFO>
    {
        Task<IEnumerable<PO_INFO>> GetPO_INFOCodes();

        Task<PO_INFO_MAPPING> Get_PO_INFO_MAPPING(string mfi_PO_CODE);

        Task<bool> Manage_PO_INFO_MAPPING(string mfi_PO_CODE, string pksf_PO_CODE);
    }
    public class PO_INFORepository : RepositoryBaseCodeFirst<PO_INFO>, IPO_INFORepository
    {
        public PO_INFORepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public async Task<IEnumerable<PO_INFO>> GetPO_INFOCodes()
        {
            var listing = new List<PO_INFO>();
            try
            {
                IQueryable<PO_INFO> query = DataContext.PO_INFOs.OrderBy(f => f.po_code);

                listing = await query.ToListAsync();
            }
            catch
            {
                return new List<PO_INFO>();
            }

            return listing;
        }

        public async Task<PO_INFO_MAPPING> Get_PO_INFO_MAPPING(string mfi_PO_CODE)
        {
            try
            {
                return await DataContext.PO_INFO_MAPPINGs.FirstOrDefaultAsync(f=>f.MFI_PO_CODE== mfi_PO_CODE);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Manage_PO_INFO_MAPPING(string mfi_PO_CODE, string pksf_PO_CODE)
        {
            try
            {
                var sqlCommand = $@"[pksf].[PO_INFO_MAPPING_Manage_PO_INFO_MAPPING] '{mfi_PO_CODE}', '{pksf_PO_CODE}'";
                await DataContext.Database.ExecuteSqlCommandAsync(sqlCommand);

                return true;
            }
            catch
            {
                return false;
            }
        }       
    }
}
