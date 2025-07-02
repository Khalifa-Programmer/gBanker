using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gBanker.Data.Repository
{
    public interface ICenterRepository : IRepository<Center>
    {
        IEnumerable<DBCenterDetailModel> GetCenterDetail(int? OrgID);
        IEnumerable<DBCenterDetailModel> GetNonCenterMeetingCenterDetail(string colday, int OfficeId);
       // IEnumerable<DBCenterDetailModel> GetCenterDetail();
    }
    public class CenterRepository : RepositoryBaseCodeFirst<Center>, ICenterRepository
    {
        public CenterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBCenterDetailModel> GetCenterDetail(int? OrgID)
        {
            var obj = DataContext.Centers.Where(x => x.IsActive == true && x.OrgID==OrgID)
                .Select(s => new DBCenterDetailModel()
                {
                    CenterID = s.CenterID,
                    CenterCode = s.CenterCode,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office.OfficeCode,
                    OfficeName = s.Office.OfficeName,
                    OfficeFullName = s.Office.OfficeCode + " " + s.Office.OfficeName,
                    CenterName = s.CenterName,
                    CenterFullName = s.CenterCode + " " + s.CenterName,
                    CenterAddress = s.CenterAddress,
                    CenterNameBng = s.CenterNameBng,
                    Organizer = s.Organizer,
                    EmployeeId = s.EmployeeId,
                    EmployeeFullName = s.Employee.EmployeeCode + ", " + s.Employee.EmpName,
                    CollectionDay = s.CollectionDay,
                    CollectionDate = s.CollectionDate,
                    GeoLocationID = s.GeoLocationID,
                    LocationName = s.GeoLocation.LocationName,
                    OperationStartDate = s.OperationStartDate,
                    CenterStatus = s.CenterStatus,
                    CenterDistance = s.CenterDistance,
                    IsActive = s.IsActive,
                    CenterTime = s.CenterTime

                });

            return obj;
        }
          
        public IEnumerable<DBCenterDetailModel> GetNonCenterMeetingCenterDetail(string colday,int OfficeId)
        {
            var obj = DataContext.Centers.Where(x => x.IsActive == true && x.CollectionDay == colday && x.OfficeID==OfficeId)
                 .Select(s => new DBCenterDetailModel()
                 {
                     CenterID = s.CenterID,
                     CenterCode = s.CenterCode,
                     OfficeID = s.OfficeID,
                     OfficeCode = s.Office.OfficeCode,
                     OfficeName = s.Office.OfficeName,
                     OfficeFullName = s.Office.OfficeCode + " " + s.Office.OfficeName,
                     CenterName = s.CenterName,
                     CenterFullName = s.CenterCode + " " + s.CenterName,
                     CenterAddress = s.CenterAddress,
                     CenterNameBng = s.CenterNameBng,
                     EmployeeId = s.EmployeeId,
                     EmployeeFullName = s.Employee.EmployeeCode + ", " + s.Employee.EmpName,
                     CollectionDay = s.CollectionDay,
                     CollectionDate = s.CollectionDate,
                     GeoLocationID = s.GeoLocationID,
                     LocationName = s.GeoLocation.LocationName,
                     OperationStartDate = s.OperationStartDate,
                     CenterStatus = s.CenterStatus,
                     CenterDistance = s.CenterDistance,
                     IsActive = s.IsActive

                 });

            return obj;
        }
    }
}
