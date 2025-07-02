using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        IEnumerable<Organization> GetOrganizationDetailPaged(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long totalCount, int? OrgID);

        Task<Organization> GetOrganizationById(long id);
        Task<bool> UpdateOrganization(Organization entity);

    }
    public class OrganizationRepository : RepositoryBaseCodeFirst<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public async Task<Organization> GetOrganizationById(long id)
        {
            try
            {
                return await DataContext.Organizations.FirstOrDefaultAsync(f => f.OrgID == id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateOrganization(Organization entity)
        {

            try
            {
                var organization = await DataContext.Organizations.FirstOrDefaultAsync(f => f.OrgID == entity.OrgID);

                if (organization == null)
                    return false;

                organization.OrganizationName = entity.OrganizationName;
                organization.OrgAddress = entity.OrgAddress;

                await DataContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Organization> GetOrganizationDetailPaged(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long totalCount, int? OrgID)
        {
            IQueryable<Organization> results = null;
            if (filterColumnName == "OrganizationCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.Organizations.Where(p => p.IsActive == true && p.OrganizationCode.Contains(filterValue) && p.OrgID == OrgID);
            else
                results = DataContext.Organizations.Where(p => p.IsActive == true && p.OrgID == OrgID);

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.OrganizationCode).Skip(startRowIndex).Take(pageSize);
            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "OrganizationName ASC")
                    obj = results.OrderBy(ord => ord.OrganizationName).Skip(startRowIndex).Take(pageSize);
                else if (jtSorting == "OrganizationCode ASC")
                    obj = results.OrderBy(ord => ord.OrganizationCode).Skip(startRowIndex).Take(pageSize);
                else if (jtSorting == "OrganizationName DESC")
                    obj = results.OrderByDescending(ord => ord.OrganizationName).Skip(startRowIndex).Take(pageSize);
                else if (jtSorting == "OrganizationCode DESC")
                    obj = results.OrderByDescending(ord => ord.OrganizationCode).Skip(startRowIndex).Take(pageSize);
            }
            else
                obj = results.OrderBy(ord => ord.OrgID).Skip(startRowIndex).Take(pageSize);


            return obj;
        }
    }
}
