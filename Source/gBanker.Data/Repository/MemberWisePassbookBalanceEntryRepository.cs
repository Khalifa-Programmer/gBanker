using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gBanker.Data.Repository
{
   
    public interface IMemberWisePassbookBalanceEntryRepository : IRepository<MemberWisePassbookBalanceEntry>
    {
        IEnumerable<PassbookEntryModel> GetMemberWisePassbookBalanceEntryList();
    }
    public class MemberWisePassbookBalanceEntryRepository : RepositoryBaseCodeFirst<MemberWisePassbookBalanceEntry>, IMemberWisePassbookBalanceEntryRepository
    {
          
        public MemberWisePassbookBalanceEntryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        
        }
        public IEnumerable<PassbookEntryModel> GetMemberWisePassbookBalanceEntryList()
        {
            try
            {
                var sqlCommand = "SELECT MemberWisePassbookBalanceEntryId , o.OfficeName,c.CenterName, CONCAT(m.FirstName,m.LastName) AS Member,P.ProductName, mwpbe.Amount FROM MemberWisePassbookBalanceEntry mwpbe" + " " +
                                      "LEFT JOIN Office o ON mwpbe.OfficeId = o.OfficeID" +" "+
                                      "LEFT JOIN Center c ON mwpbe.CenterId = c.CenterID" +" "+
                                      "LEFT JOIN Member m ON mwpbe.MemberId = m.MemberID" +" "+
                                      "LEFT JOIN Product p ON mwpbe.ProductId = p.ProductID";

                var results = DataContext.Database.SqlQuery<PassbookEntryModel>(sqlCommand).ToList();

                return results;
            }
            catch (Exception ex)
            {
                return new List<PassbookEntryModel>();
            }
        }

    }
}
