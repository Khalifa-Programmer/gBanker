using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Data.Repository
{
    public interface IOfficeRepository : IRepository<Office>
    {
        IEnumerable<DBOfficeDetailModel> GetOfficeDetail();
        IEnumerable<DBOfficeDetailModel> GetHeadOffice();
        IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode,int? orgiD);
        IEnumerable<DBOfficeDetailModel> GetAllAreaOfficeForZone(string headofficeCode, string zoneCode, int? orgiD);
        IEnumerable<DBOfficeDetailModel> GetAllBranchesForArea(string headofficeCode, string zoneCode, string areaCode, int? orgiD);
        int GetAllOfficeCount();
        IEnumerable<DBOfficeDetailModel> GetAllZoneOffice1(string headofficeCode, int? orgiD);
        //object GetMany(Expression<Func<DBOfficeDetailModel, bool>> where);
    }
    public class OfficeRepository : RepositoryBaseCodeFirst<Office>, IOfficeRepository
    {
        public OfficeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBOfficeDetailModel> GetOfficeDetail()
        {
            var obj = DataContext.Offices.Where(x => x.IsActive == true )
                .Select(s => new DBOfficeDetailModel()
                {
                    OfficeID = s.OfficeID,
                    OfficeCode = s.OfficeCode,
                    OfficeName = s.OfficeName,
                    OfficeLevel = s.OfficeLevel,
                    FirstLevel = s.FirstLevel,
                    SecondLevel = s.SecondLevel,
                    ThirdLevel = s.ThirdLevel,
                    FourthLevel = s.FourthLevel,
                    OperationStartDate = s.OperationStartDate,
                    OfficeAddress = s.OfficeAddress,
                    PostCode = s.PostCode,
                    GeoLocationID = s.GeoLocationID,
                    LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                    Email = s.Email,
                    Phone = s.Phone
                });

            return obj;
        }


        public IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode, int? orgiD)
        {
            var zoneOffices = DataContext.Offices.Where(x => x.IsActive == true && x.OfficeLevel == 2 && x.OrgID==orgiD )
                .Select(s => new DBOfficeDetailModel()
                {
                    OfficeID = s.OfficeID,
                    OfficeCode = s.OfficeCode,
                    OfficeName = s.OfficeName,
                    OfficeLevel = s.OfficeLevel,
                    FirstLevel = s.FirstLevel,
                    SecondLevel = s.SecondLevel,
                    ThirdLevel = s.ThirdLevel,
                    FourthLevel = s.FourthLevel,
                    OperationStartDate = s.OperationStartDate,
                    OfficeAddress = s.OfficeAddress,
                    PostCode = s.PostCode,
                    GeoLocationID = s.GeoLocationID,
                    LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                    Email = s.Email,
                    Phone = s.Phone
                });
            return zoneOffices;
        }

        public IEnumerable<DBOfficeDetailModel> GetAllAreaOfficeForZone(string headofficeCode, string zoneCode, int? orgiD)
        {
            var areaOffices = DataContext.Offices.Where(x => x.IsActive == true && x.SecondLevel == zoneCode && x.OfficeLevel == 3 && x.OrgID==orgiD)
                .Select(s => new DBOfficeDetailModel()
                {
                    OfficeID = s.OfficeID,
                    OfficeCode = s.OfficeCode,
                    OfficeName = s.OfficeName,
                    OfficeLevel = s.OfficeLevel,
                    FirstLevel = s.FirstLevel,
                    SecondLevel = s.SecondLevel,
                    ThirdLevel = s.ThirdLevel,
                    FourthLevel = s.FourthLevel,
                    OperationStartDate = s.OperationStartDate,
                    OfficeAddress = s.OfficeAddress,
                    PostCode = s.PostCode,
                    GeoLocationID = s.GeoLocationID,
                    LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                    Email = s.Email,
                    Phone = s.Phone
                });
            return areaOffices;
        }

        public IEnumerable<DBOfficeDetailModel> GetAllBranchesForArea(string headofficeCode, string zoneCode, string areaCode, int? orgiD)
        {
             IQueryable<DBOfficeDetailModel> branchOffices=null;
            if (headofficeCode != null && zoneCode =="0" && areaCode == "0")
            {
                 branchOffices = DataContext.Offices.Where(x => x.IsActive == true && x.OrgID == orgiD)
               .Select(s => new DBOfficeDetailModel()
               {
                   OfficeID = s.OfficeID,
                   OfficeCode = s.OfficeCode,
                   OfficeName = s.OfficeName,
                   OfficeLevel = s.OfficeLevel,
                   FirstLevel = s.FirstLevel,
                   SecondLevel = s.SecondLevel,
                   ThirdLevel = s.ThirdLevel,
                   FourthLevel = s.FourthLevel,
                   OperationStartDate = s.OperationStartDate,
                   OfficeAddress = s.OfficeAddress,
                   PostCode = s.PostCode,
                   GeoLocationID = s.GeoLocationID,
                   LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                   Email = s.Email,
                   Phone = s.Phone
               });
              

            }
            else if (headofficeCode != null && zoneCode != "0" && areaCode == "0")
            {
                 branchOffices = DataContext.Offices.Where(x => x.IsActive == true && x.OrgID == orgiD && x.SecondLevel == zoneCode)
                   .Select(s => new DBOfficeDetailModel()
                   {
                       OfficeID = s.OfficeID,
                       OfficeCode = s.OfficeCode,
                       OfficeName = s.OfficeName,
                       OfficeLevel = s.OfficeLevel,
                       FirstLevel = s.FirstLevel,
                       SecondLevel = s.SecondLevel,
                       ThirdLevel = s.ThirdLevel,
                       FourthLevel = s.FourthLevel,
                       OperationStartDate = s.OperationStartDate,
                       OfficeAddress = s.OfficeAddress,
                       PostCode = s.PostCode,
                       GeoLocationID = s.GeoLocationID,
                       LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                       Email = s.Email,
                       Phone = s.Phone
                   });
               // return branchOffices; 

            }
            else if (headofficeCode != null && zoneCode != "0" && areaCode != "0")
            {
                 branchOffices = DataContext.Offices.Where(x => x.IsActive == true && x.OrgID == orgiD && x.SecondLevel == zoneCode && x.ThirdLevel == areaCode && x.OfficeLevel == 4)
                   .Select(s => new DBOfficeDetailModel()
                   {
                       OfficeID = s.OfficeID,
                       OfficeCode = s.OfficeCode,
                       OfficeName = s.OfficeName,
                       OfficeLevel = s.OfficeLevel,
                       FirstLevel = s.FirstLevel,
                       SecondLevel = s.SecondLevel,
                       ThirdLevel = s.ThirdLevel,
                       FourthLevel = s.FourthLevel,
                       OperationStartDate = s.OperationStartDate,
                       OfficeAddress = s.OfficeAddress,
                       PostCode = s.PostCode,
                       GeoLocationID = s.GeoLocationID,
                       LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                       Email = s.Email,
                       Phone = s.Phone
                   });
                //return branchOffices; 

            }
            else if (headofficeCode != null && zoneCode == "0" && areaCode != "0")
            {
                branchOffices = DataContext.Offices.Where(x => x.IsActive == true && x.OrgID == orgiD )
                  .Select(s => new DBOfficeDetailModel()
                  {
                      OfficeID = s.OfficeID,
                      OfficeCode = s.OfficeCode,
                      OfficeName = s.OfficeName,
                      OfficeLevel = s.OfficeLevel,
                      FirstLevel = s.FirstLevel,
                      SecondLevel = s.SecondLevel,
                      ThirdLevel = s.ThirdLevel,
                      FourthLevel = s.FourthLevel,
                      OperationStartDate = s.OperationStartDate,
                      OfficeAddress = s.OfficeAddress,
                      PostCode = s.PostCode,
                      GeoLocationID = s.GeoLocationID,
                      LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                      Email = s.Email,
                      Phone = s.Phone
                  });
                //return branchOffices; 

            }
            return branchOffices; 
            //var branchOffices = DataContext.Offices.Where(x => x.IsActive == true  && x.SecondLevel == zoneCode && x.ThirdLevel == areaCode && x.OfficeLevel == 4 && x.OrgID==orgiD)
            //     .Select(s => new DBOfficeDetailModel()
            //     {
            //         OfficeID = s.OfficeID,
            //         OfficeCode = s.OfficeCode,
            //         OfficeName = s.OfficeName,
            //         OfficeLevel = s.OfficeLevel,
            //         FirstLevel = s.FirstLevel,
            //         SecondLevel = s.SecondLevel,
            //         ThirdLevel = s.ThirdLevel,
            //         FourthLevel = s.FourthLevel,
            //         OperationStartDate = s.OperationStartDate,
            //         OfficeAddress = s.OfficeAddress,
            //         PostCode = s.PostCode,
            //         GeoLocationID = s.GeoLocationID,
            //         LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
            //         Email = s.Email,
            //         Phone = s.Phone
            //     });
            //return branchOffices; ;
        }


        public int GetAllOfficeCount()
        {
            return DataContext.Offices.Where(x => x.IsActive == true && x.OfficeLevel==4).Count();
        }


        public IEnumerable<DBOfficeDetailModel> GetHeadOffice()
        {
            var headOffices = DataContext.Offices.Where(x => x.IsActive == true && x.OfficeLevel == 1)
                .Select(s => new DBOfficeDetailModel()
                {
                    OfficeID = s.OfficeID,
                    OfficeCode = s.OfficeCode,
                    OfficeName = s.OfficeName,
                    OfficeLevel = s.OfficeLevel,
                    FirstLevel = s.FirstLevel,
                    SecondLevel = s.SecondLevel,
                    ThirdLevel = s.ThirdLevel,
                    FourthLevel = s.FourthLevel,
                    OperationStartDate = s.OperationStartDate,
                    OfficeAddress = s.OfficeAddress,
                    PostCode = s.PostCode,
                    GeoLocationID = s.GeoLocationID,
                    LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                    Email = s.Email,
                    Phone = s.Phone
                });
            return headOffices;
        }


        public IEnumerable<DBOfficeDetailModel> GetAllZoneOffice1(string headofficeCode, int? orgiD)
        {
            var zoneOffices = DataContext.Offices.Where(x => x.IsActive == true && x.OfficeLevel == 1 && x.OrgID == orgiD)
               .Select(s => new DBOfficeDetailModel()
               {
                   OfficeID = s.OfficeID,
                   OfficeCode = s.OfficeCode,
                   OfficeName = s.OfficeName
                   //,
                   //OfficeLevel = s.OfficeLevel,
                   //FirstLevel = s.FirstLevel,
                   //SecondLevel = s.SecondLevel,
                   //ThirdLevel = s.ThirdLevel,
                   //FourthLevel = s.FourthLevel,
                   //OperationStartDate = s.OperationStartDate,
                   //OfficeAddress = s.OfficeAddress,
                   //PostCode = s.PostCode,
                   //GeoLocationID = s.GeoLocationID,
                   //LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                   //Email = s.Email,
                   //Phone = s.Phone
               });
            return zoneOffices;
        }
    }
}
