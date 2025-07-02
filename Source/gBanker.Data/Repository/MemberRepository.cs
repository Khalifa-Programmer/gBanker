using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IMemberRepository : IRepository<Member>
    {
        IEnumerable<DBMemberDetailModel> GetMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<DBMemberDetailModel> GetEligibleMemberDetail(int? orgID,int? officeID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<DBMemberDetailModel> GetApprovalMember(int? orgID, int? officeID, out long TotCount);
        int GetTotalOrganizationMember();
        IEnumerable<Member> SearchMember(int OfficeID,int OrgID);
        IEnumerable<DBMemberDetailModel> GetApprovedMemberForTransfer(int? orgID, int? officeID, out long TotCount);

        IEnumerable<DBMemberDetailModel> GetMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount,short empID);
        IEnumerable<DBMemberDetailModel> GetEligibleMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount, short empID);
    }
    public class MemberRepository : RepositoryBaseCodeFirst<Member>, IMemberRepository
    {
        public MemberRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBMemberDetailModel> GetMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<Member> results = null;
            if (TypeFilterColumn == "V")
            {
                if (filterColumnName == "MemberCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID==orgID && x.OfficeID == officeID && x.MemberCode.Contains(filterValue));
                else if (filterColumnName == "MemberName")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && (x.FirstName + " " + x.MiddleName + " " + x.LastName).Contains(filterValue));
                else if (filterColumnName == "CenterCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Center.CenterCode.Contains(filterValue));
                else if (filterColumnName == "GroupCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Group.GroupCode == filterValue);
                else if (filterColumnName == "NationalID")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID  && x.NationalID.Contains(filterValue));
                else if (filterColumnName == "PhoneNo")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.PhoneNo.Contains(filterValue));


                else
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID);
            }
            else
            {
                if (filterColumnName == "MemberCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.MemberCode.Contains(filterValue) && x.MemberStatus == TypeFilterColumn);
                else if (filterColumnName == "MemberName")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && (x.FirstName + " " + x.MiddleName + " " + x.LastName).Contains(filterValue) && x.MemberStatus == TypeFilterColumn);
                else if (filterColumnName == "CenterCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Center.CenterCode.Contains(filterValue) && x.MemberStatus == TypeFilterColumn);
                else if (filterColumnName == "GroupCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Group.GroupCode == filterValue && x.MemberStatus == TypeFilterColumn);
                else if (filterColumnName == "NationalID")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID  && x.NationalID.Contains(filterValue));
                else if (filterColumnName == "PhoneNo")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.PhoneNo.Contains(filterValue));


                else
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.MemberStatus == TypeFilterColumn);
            }
            TotCount = results.LongCount();
            var obj = results.OrderBy(o=> o.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBMemberDetailModel()
                {
                    //RowSl = 0,
                    MemberID = s.MemberID,
                    MemberCode = s.MemberCode,
                    OldMemberCode = s.OldMemberCode,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office.OfficeCode,
                    OfficeName = s.Office.OfficeName,
                    CenterID = s.CenterID,
                    CenterCode = s.Center.CenterCode,
                    CenterName = s.Center.CenterCode + ", " + s.Center.CenterName,
                    GroupID = s.GroupID,
                    GroupCode = s.Group.GroupCode,
                    //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                    FullName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                    FirstName = s.FirstName,
                    MiddleName = s.MiddleName,
                    LastName = s.LastName,
                    AddressLine1 = s.AddressLine1,
                    AddressLine2 = s.AddressLine2,
                    RefereeName = s.RefereeName,
                    BirthDate = s.BirthDate,
                    JoinDate = s.JoinDate,
                    ReleaseDate = s.ReleaseDate,
                    Gender = s.Gender,
                    NationalID = s.NationalID,
                    Location = s.Location,
                    //LocationName = s.ge
                    //GeoLocationID { get; set; }
                    //LocationName
                    MemberCategoryID = s.MemberCategoryID,
                    MemberCategoryCode = s.MemberCategory.CategoryShortName,
                    CategoryName = s.MemberCategory.CategoryName,
                    //CategoryShortName
                    MemberStatus = s.MemberStatus,
                    //ReleaseDate = s.ReleaseDate,
                    City = s.City,
                    StateName = s.StateName,
                    ZipCode = s.ZipCode,
                    CountryOfIssue = s.CountryOfIssue,
                    NIDComments = s.NIDComments,
                    IDType = s.IDType,
                    Race = s.Race,
                    Ethnicity = s.Ethnicity,
                    Email = s.Email,
                    PhoneNo = s.PhoneNo,
                    nsAccountNo = s.nsAccountNo,
                    MemberType = s.MemberType,
                    MemCategory = s.MemCategory
                });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "MemberCode ASC")
                    return obj.OrderBy(o => o.MemberCode);
                else if (jtSorting == "MemberCode DESC")
                    return obj.OrderByDescending(o => o.MemberCode);
                else if (jtSorting == "CenterName ASC")
                    return obj.OrderBy(o => o.CenterCode);
                else if (jtSorting == "CenterName DESC")
                    return obj.OrderByDescending(o => o.CenterCode);                
                else if (jtSorting == "GroupCode ASC")
                    return obj.OrderBy(o => o.GroupCode);
                else if (jtSorting == "GroupCode DESC")
                    return obj.OrderByDescending(o => o.GroupCode);
                else if (jtSorting == "FullName ASC")
                    return obj.OrderBy(o => o.FullName);
                else if (jtSorting == "FullName DESC")
                    return obj.OrderByDescending(o => o.FullName);
                else
                    return obj.OrderBy(o => o.MemberCode);
            }
            else
                return obj.OrderBy(o => o.MemberCode);            
        }
        public IEnumerable<DBMemberDetailModel> GetEligibleMemberDetail(int? orgID,int? officeID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<Member> results = null;
            if (filterColumnName == "MemberCode")
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID==orgID && x.MemberStatus == "0" && x.OfficeID == officeID && x.MemberCode.Contains(filterValue));
            else if (filterColumnName == "MemberName")
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && (x.FirstName + " " + x.MiddleName + " " + x.LastName).Contains(filterValue));
            else if (filterColumnName == "CenterCode")
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.MemberStatus == "0" && x.OfficeID == officeID && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "GroupCode")
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.MemberStatus == "0" && x.OfficeID == officeID && x.Group.GroupCode == filterValue);
            else
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.MemberStatus == "0" && x.OfficeID == officeID);
            TotCount = results.LongCount();
            var obj = results.OrderBy(o => o.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBMemberDetailModel()
            {
                MemberID = s.MemberID,
                MemberCode = s.MemberCode,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office.OfficeCode,
                OfficeName = s.Office.OfficeName,
                CenterID = s.CenterID,
                CenterCode = s.Center.CenterCode,
                CenterName = s.Center.CenterCode + ", " + s.Center.CenterName,
                GroupID = s.GroupID,
                GroupCode = s.Group.GroupCode,
                //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                FullName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                LastName = s.LastName,
                AddressLine1 = s.AddressLine1,
                AddressLine2 = s.AddressLine2,
                RefereeName = s.RefereeName,
                BirthDate = s.BirthDate,
                JoinDate = s.JoinDate,
                Gender = s.Gender,
                NationalID = s.NationalID,
                Location = s.Location,
                //LocationName = s.ge
                //GeoLocationID { get; set; }
                //LocationName
                MemberCategoryID = s.MemberCategoryID,
                MemberCategoryCode = s.MemberCategory.CategoryShortName,
                CategoryName = s.MemberCategory.CategoryName,
                //CategoryShortName
                MemberStatus = s.MemberStatus,
                ReleaseDate = s.ReleaseDate,
                City = s.City,
                StateName = s.StateName,
                ZipCode = s.ZipCode,
                CountryOfIssue = s.CountryOfIssue,
                NIDComments = s.NIDComments,
                IDType = s.IDType,
                Race = s.Race,
                Ethnicity = s.Ethnicity,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                nsAccountNo = s.nsAccountNo,
                MemberType = s.MemberType,
                MemCategory = s.MemCategory
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "MemberCode ASC")
                    return obj.OrderBy(o => o.MemberCode);
                else if (jtSorting == "MemberCode DESC")
                    return obj.OrderByDescending(o => o.MemberCode);
                else if (jtSorting == "CenterName ASC")
                    return obj.OrderBy(o => o.CenterCode);
                else if (jtSorting == "CenterName DESC")
                    return obj.OrderByDescending(o => o.CenterCode);
                else if (jtSorting == "GroupCode ASC")
                    return obj.OrderBy(o => o.GroupCode);
                else if (jtSorting == "GroupCode DESC")
                    return obj.OrderByDescending(o => o.GroupCode);
                else if (jtSorting == "FullName ASC")
                    return obj.OrderBy(o => o.FullName);
                else if (jtSorting == "FullName DESC")
                    return obj.OrderByDescending(o => o.FullName);
                else
                    return obj.OrderBy(o => o.MemberCode);
            }
            else
                return obj.OrderBy(o => o.MemberCode);
        }
        public IEnumerable<DBMemberDetailModel> GetApprovalMember(int? orgID,int? officeID, out long TotCount)
        {
            IQueryable<Member> results = null;            
            results = DataContext.Members.Where(x => x.IsActive == true && x.MemberStatus == "0" && x.OfficeID == officeID && x.OrgID==orgID);
            TotCount = results.LongCount();
            var obj = results.OrderBy(o => o.MemberCode).Select(s => new DBMemberDetailModel()
            {
                MemberID = s.MemberID,
                MemberCode = s.MemberCode,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office.OfficeCode,
                OfficeName = s.Office.OfficeName,
                CenterID = s.CenterID,
                CenterCode = s.Center.CenterCode,
                CenterName = s.Center.CenterCode + ", " + s.Center.CenterName,
                GroupID = s.GroupID,
                GroupCode = s.Group.GroupCode,
                //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                FullName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                LastName = s.LastName,
                AddressLine1 = s.AddressLine1,
                AddressLine2 = s.AddressLine2,
                RefereeName = s.RefereeName,
                BirthDate = s.BirthDate,
                JoinDate = s.JoinDate,
                Gender = s.Gender,
                NationalID = s.NationalID,
                Location = s.Location,
                //LocationName = s.ge
                //GeoLocationID { get; set; }
                //LocationName
                MemberCategoryID = s.MemberCategoryID,
                MemberCategoryCode = s.MemberCategory.CategoryShortName,
                CategoryName = s.MemberCategory.CategoryName,
                //CategoryShortName
                MemberStatus = s.MemberStatus,
                ReleaseDate = s.ReleaseDate,
                City = s.City,
                StateName = s.StateName,
                ZipCode = s.ZipCode,
                CountryOfIssue = s.CountryOfIssue,
                NIDComments = s.NIDComments,
                IDType = s.IDType,
                Race = s.Race,
                Ethnicity = s.Ethnicity,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                nsAccountNo = s.nsAccountNo,
                MemberType = s.MemberType,
                MemCategory = s.MemCategory
            });
            
            return obj.OrderBy(o => o.MemberCode);
        }

        public IEnumerable<DBMemberDetailModel> GetApprovedMemberForTransfer(int? orgID, int? officeID, out long TotCount)
        {
            IQueryable<Member> results = null;
            results = DataContext.Members.Where(x => x.IsActive == true && x.MemberStatus == "1" && x.OfficeID == officeID && x.OrgID == orgID);
            TotCount = results.LongCount();
            var obj = results.OrderBy(o => o.MemberCode).Select(s => new DBMemberDetailModel()
            {
                MemberID = s.MemberID,
                MemberCode = s.MemberCode,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office.OfficeCode,
                OfficeName = s.Office.OfficeName,
                CenterID = s.CenterID,
                CenterCode = s.Center.CenterCode,
                CenterName = s.Center.CenterCode + ", " + s.Center.CenterName,
                GroupID = s.GroupID,
                GroupCode = s.Group.GroupCode,
                //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                FullName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                LastName = s.LastName,
                AddressLine1 = s.AddressLine1,
                AddressLine2 = s.AddressLine2,
                RefereeName = s.RefereeName,
                BirthDate = s.BirthDate,
                JoinDate = s.JoinDate,
                Gender = s.Gender,
                NationalID = s.NationalID,
                Location = s.Location,
                //LocationName = s.ge
                //GeoLocationID { get; set; }
                //LocationName
                MemberCategoryID = s.MemberCategoryID,
                MemberCategoryCode = s.MemberCategory.CategoryShortName,
                CategoryName = s.MemberCategory.CategoryName,
                //CategoryShortName
                MemberStatus = s.MemberStatus,
                ReleaseDate = s.ReleaseDate,
                City = s.City,
                StateName = s.StateName,
                ZipCode = s.ZipCode,
                CountryOfIssue = s.CountryOfIssue,
                NIDComments = s.NIDComments,
                IDType = s.IDType,
                Race = s.Race,
                Ethnicity = s.Ethnicity,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                nsAccountNo = s.nsAccountNo,
                MemberType = s.MemberType,
                MemCategory = s.MemCategory
            });

            return obj.OrderBy(o => o.MemberCode);
        }
        public int GetTotalOrganizationMember()
        {
            return DataContext.Members.Where(x => x.IsActive == true).Count();
        }


        public IEnumerable<Member> SearchMember(int OfficeID, int OrgID)
        {
            IQueryable<Member> results = null;
            results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID==OrgID  && x.MemberStatus == "1" && x.OfficeID == OfficeID && x.MemberStatus != "4");
            return results;
        }


        public IEnumerable<DBMemberDetailModel> GetMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount, short empID)
        {


            IQueryable<Member> results = null;
            if (TypeFilterColumn == "V")
            {
                if (filterColumnName == "MemberCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.MemberCode.Contains(filterValue) && x.Center.EmployeeId==empID);
                else if (filterColumnName == "MemberName")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && (x.FirstName + " " + x.MiddleName + " " + x.LastName).Contains(filterValue) &&  x.Center.EmployeeId==empID);
                else if (filterColumnName == "CenterCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Center.CenterCode.Contains(filterValue) && x.Center.EmployeeId == empID);
                else if (filterColumnName == "GroupCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Group.GroupCode == filterValue && x.Center.EmployeeId == empID);
                else if (filterColumnName == "NationalID")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID  && x.NationalID.Contains(filterValue) && x.Center.EmployeeId == empID);
                else if (filterColumnName == "PhoneNo")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.PhoneNo.Contains(filterValue));


                else
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Center.EmployeeId == empID);
            }
            else
            {
                if (filterColumnName == "MemberCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.MemberCode.Contains(filterValue) && x.MemberStatus == TypeFilterColumn && x.Center.EmployeeId == empID);
                else if (filterColumnName == "MemberName")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && (x.FirstName + " " + x.MiddleName + " " + x.LastName).Contains(filterValue) && x.MemberStatus == TypeFilterColumn && x.Center.EmployeeId == empID);
                else if (filterColumnName == "CenterCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Center.CenterCode.Contains(filterValue) && x.MemberStatus == TypeFilterColumn && x.Center.EmployeeId == empID);
                else if (filterColumnName == "GroupCode")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.Group.GroupCode == filterValue && x.MemberStatus == TypeFilterColumn && x.Center.EmployeeId == empID);
                else if (filterColumnName == "NationalID")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID  && x.NationalID.Contains(filterValue) && x.Center.EmployeeId == empID);
                else if (filterColumnName == "PhoneNo")
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.PhoneNo.Contains(filterValue));


                else
                    results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && x.MemberStatus == TypeFilterColumn && x.Center.EmployeeId == empID);
            }
            TotCount = results.LongCount();
            var obj = results.OrderBy(o => o.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBMemberDetailModel()
            {
                //RowSl = 0,
                MemberID = s.MemberID,
                MemberCode = s.MemberCode,
                OldMemberCode = s.OldMemberCode,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office.OfficeCode,
                OfficeName = s.Office.OfficeName,
                CenterID = s.CenterID,
                CenterCode = s.Center.CenterCode,
                CenterName = s.Center.CenterCode + ", " + s.Center.CenterName,
                GroupID = s.GroupID,
                GroupCode = s.Group.GroupCode,
                //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                FullName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                LastName = s.LastName,
                AddressLine1 = s.AddressLine1,
                AddressLine2 = s.AddressLine2,
                RefereeName = s.RefereeName,
                BirthDate = s.BirthDate,
                JoinDate = s.JoinDate,
                ReleaseDate = s.ReleaseDate,
                Gender = s.Gender,
                NationalID = s.NationalID,
                Location = s.Location,
                //LocationName = s.ge
                //GeoLocationID { get; set; }
                //LocationName
                MemberCategoryID = s.MemberCategoryID,
                MemberCategoryCode = s.MemberCategory.CategoryShortName,
                CategoryName = s.MemberCategory.CategoryName,
                //CategoryShortName
                MemberStatus = s.MemberStatus,
                //ReleaseDate = s.ReleaseDate,
                City = s.City,
                StateName = s.StateName,
                ZipCode = s.ZipCode,
                CountryOfIssue = s.CountryOfIssue,
                NIDComments = s.NIDComments,
                IDType = s.IDType,
                Race = s.Race,
                Ethnicity = s.Ethnicity,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                nsAccountNo = s.nsAccountNo,
                MemberType = s.MemberType,
                MemCategory = s.MemCategory
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "MemberCode ASC")
                    return obj.OrderBy(o => o.MemberCode);
                else if (jtSorting == "MemberCode DESC")
                    return obj.OrderByDescending(o => o.MemberCode);
                else if (jtSorting == "CenterName ASC")
                    return obj.OrderBy(o => o.CenterCode);
                else if (jtSorting == "CenterName DESC")
                    return obj.OrderByDescending(o => o.CenterCode);
                else if (jtSorting == "GroupCode ASC")
                    return obj.OrderBy(o => o.GroupCode);
                else if (jtSorting == "GroupCode DESC")
                    return obj.OrderByDescending(o => o.GroupCode);
                else if (jtSorting == "FullName ASC")
                    return obj.OrderBy(o => o.FullName);
                else if (jtSorting == "FullName DESC")
                    return obj.OrderByDescending(o => o.FullName);
                else
                    return obj.OrderBy(o => o.MemberCode);
            }
            else
                return obj.OrderBy(o => o.MemberCode);      
        }


        public IEnumerable<DBMemberDetailModel> GetEligibleMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount, short empID)
        {
            IQueryable<Member> results = null;
            if (filterColumnName == "MemberCode")
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.MemberStatus == "0" && x.OfficeID == officeID && x.MemberCode.Contains(filterValue) && x.Center.EmployeeId==empID);
            else if (filterColumnName == "MemberName")
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.OfficeID == officeID && (x.FirstName + " " + x.MiddleName + " " + x.LastName).Contains(filterValue) && x.Center.EmployeeId == empID);
            else if (filterColumnName == "CenterCode")
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.MemberStatus == "0" && x.OfficeID == officeID && x.Center.CenterCode.Contains(filterValue) && x.Center.EmployeeId == empID);
            else if (filterColumnName == "GroupCode")
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.MemberStatus == "0" && x.OfficeID == officeID && x.Group.GroupCode == filterValue && x.Center.EmployeeId == empID);
            else
                results = DataContext.Members.Where(x => x.IsActive == true && x.OrgID == orgID && x.MemberStatus == "0" && x.OfficeID == officeID && x.Center.EmployeeId == empID);
            TotCount = results.LongCount();
            var obj = results.OrderBy(o => o.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBMemberDetailModel()
            {
                MemberID = s.MemberID,
                MemberCode = s.MemberCode,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office.OfficeCode,
                OfficeName = s.Office.OfficeName,
                CenterID = s.CenterID,
                CenterCode = s.Center.CenterCode,
                CenterName = s.Center.CenterCode + ", " + s.Center.CenterName,
                GroupID = s.GroupID,
                GroupCode = s.Group.GroupCode,
                //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                FullName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                LastName = s.LastName,
                AddressLine1 = s.AddressLine1,
                AddressLine2 = s.AddressLine2,
                RefereeName = s.RefereeName,
                BirthDate = s.BirthDate,
                JoinDate = s.JoinDate,
                Gender = s.Gender,
                NationalID = s.NationalID,
                Location = s.Location,
                //LocationName = s.ge
                //GeoLocationID { get; set; }
                //LocationName
                MemberCategoryID = s.MemberCategoryID,
                MemberCategoryCode = s.MemberCategory.CategoryShortName,
                CategoryName = s.MemberCategory.CategoryName,
                //CategoryShortName
                MemberStatus = s.MemberStatus,
                ReleaseDate = s.ReleaseDate,
                City = s.City,
                StateName = s.StateName,
                ZipCode = s.ZipCode,
                CountryOfIssue = s.CountryOfIssue,
                NIDComments = s.NIDComments,
                IDType = s.IDType,
                Race = s.Race,
                Ethnicity = s.Ethnicity,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                nsAccountNo = s.nsAccountNo,
                MemberType = s.MemberType,
                MemCategory = s.MemCategory
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "MemberCode ASC")
                    return obj.OrderBy(o => o.MemberCode);
                else if (jtSorting == "MemberCode DESC")
                    return obj.OrderByDescending(o => o.MemberCode);
                else if (jtSorting == "CenterName ASC")
                    return obj.OrderBy(o => o.CenterCode);
                else if (jtSorting == "CenterName DESC")
                    return obj.OrderByDescending(o => o.CenterCode);
                else if (jtSorting == "GroupCode ASC")
                    return obj.OrderBy(o => o.GroupCode);
                else if (jtSorting == "GroupCode DESC")
                    return obj.OrderByDescending(o => o.GroupCode);
                else if (jtSorting == "FullName ASC")
                    return obj.OrderBy(o => o.FullName);
                else if (jtSorting == "FullName DESC")
                    return obj.OrderByDescending(o => o.FullName);
                else
                    return obj.OrderBy(o => o.MemberCode);
            }
            else
                return obj.OrderBy(o => o.MemberCode);
        }
    }
}
