using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IMemberService : IServiceBase<Member>
    {
        //IEnumerable<Member> GetByOfficeIdAll(int officeId, int OrgID);
        IEnumerable<Member> GetByOfficeAll(int officeId, int OrgID);



        IEnumerable<Member> GetByCenterIdAll(int CenterID, int officeId, int OrgID);

        IEnumerable<ValidationResult> CheckSmartCardIdEdit(string nid, long mid);
        IEnumerable<ValidationResult> CheckSmartCardId(string nid);
        IEnumerable<ValidationResult> CheckImageSize();
        IEnumerable<ValidationResult> IsValidMember(Member member);
        IEnumerable<Member> SearchMember(int OfficeID, int OrgID);
        IEnumerable<Member> GetByCenterId(int CenterID,int officeId,int OrgID);
        IEnumerable<Member> GetByOfficeId(int officeId, int OrgID);
        IEnumerable<DBMemberDetailModel> GetMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<DBMemberDetailModel> GetEligibleMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<DBMemberDetailModel> GetApprovalMember(int? orgID, int? officeID, out long TotCount);
        IEnumerable<ValidationResult> CheckMemberNationalId(string nid);
        IEnumerable<ValidationResult> CheckMemberPhoneNo(string PhoneNo);
        IEnumerable<ValidationResult> CheckMemberNationalIdEdit(string nid, long mid);

        IEnumerable<ValidationResult> CheckMemberPhoneNoEdit(string nid, long mid);
        int GetTotalOrganizationMember();
        Member GetByMemeberCode(string MemCode);
        Member GetByMemberId(Int64 memId);
        IEnumerable<ValidationResult> CheckMemberUnique(Member member);
        IEnumerable<DBMemberDetailModel> GetApprovedMemberForTransfer(int? orgID, int? officeID, out long TotCount);
        IEnumerable<DBMemberDetailModel> GetMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount, short empID);
        IEnumerable<DBMemberDetailModel> GetEligibleMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount, short empID);
    }
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly ILoanSummaryRepository loanRepository;

        public MemberService(IMemberRepository repository, ILoanSummaryRepository loanRepository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.loanRepository = loanRepository;
        }


        public IEnumerable<Member> GetByOfficeAll(int officeId, int OrgID)
        {
            var members = repository.GetMany(m => m.IsActive == true && m.MemberStatus == "2" && m.OfficeID == officeId && m.OrgID == OrgID).OrderBy(mbr => mbr.MemberCode);
            // var members = repository.GetMany(m => m.CenterID == CenterID).OrderBy(mbr => mbr.MemberCode);
            return members;
        }
        public IEnumerable<Member> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberCode);
            return entities;
        }

        public Member GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Member> GetByCenterIdAll(int CenterID, int officeId, int OrgID)
        {
            var members = repository.GetMany(m => m.CenterID == CenterID && m.IsActive == true && m.MemberStatus != "5" && m.OfficeID == officeId && m.OrgID == OrgID).OrderBy(mbr => mbr.MemberCode);
            // var members = repository.GetMany(m => m.CenterID == CenterID).OrderBy(mbr => mbr.MemberCode);
            return members;
        }
        public Member GetByMemberId(Int64 memId)
        {
            var entity = repository.Get(e => e.MemberID == memId);
            return entity;
        }

        public Member Create(Member objectToCreate)
        {
            repository.Add(objectToCreate);
            //var loanSummary = new LoanSummary();
            //loanSummary.MemberID = objectToCreate.MemberID;            
            //loanRepository.Add(loanSummary);
            Save();
            return objectToCreate;
        }

        public void Update(Member objectToUpdate)
        {
            //var entity = repository.GetById(id);
            //repository.Delete(entity);
            //Save();
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public IEnumerable<ValidationResult> IsValidMember(Member member)
        {
            if (member.NationalID!=null)
            {

            
            
            var entity = repository.Get(p => p.MemberStatus=="1" && p.NationalID == member.NationalID);
            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Member", "Duplicate Member.");
            }
            }
        }

        public IEnumerable<ValidationResult> CheckSmartCardIdEdit(string nid, long mid)
        {
            var entity = repository.Get(p => (p.MemberStatus == "1" || p.MemberStatus == "0") && p.SmartCard == nid && p.MemberID != mid);

            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Member", "Duplicate Smart Card Number.");
            }
        }



        public IEnumerable<ValidationResult> CheckSmartCardId(string nid)
        {
            var entity = repository.Get(p => (p.MemberStatus == "1" || p.MemberStatus == "0") && p.SmartCard == nid);

            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Member", "Duplicate SmartCard Id .");
            }
        }

        public IEnumerable<ValidationResult> CheckMemberUnique(Member member)
        {
            var entity = repository.Get(p => p.MemberStatus=="1" && p.PhoneNo == member.PhoneNo);
            
            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Member", "Duplicate National Id Or Phone Number.");
            }
        }

        public IEnumerable<ValidationResult> CheckMemberNationalId(string nid)
        {
            var entity = repository.Get(p => (p.MemberStatus == "1" || p.MemberStatus == "0") && p.NationalID == nid);

            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Member", "Duplicate National Id .");
            }
        }
        public IEnumerable<ValidationResult> CheckMemberPhoneNo(string PhoneNo)
        {
            var entity = repository.Get(p => p.MemberStatus == "1" && p.PhoneNo == PhoneNo);

            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Member", "Duplicate Phone Number.");
            }
        }
        public void Save()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<Member> SearchMember(int OfficeID,int OrgID)
        {
            //return repository.GetMany(g => g.OfficeID == OfficeID && g.IsActive == true && g.MemberStatus == "1").OrderBy(g => g.MemberID);
            return repository.SearchMember(OfficeID,OrgID);
        }


        public IEnumerable<Member> GetByCenterId(int CenterID, int officeId, int OrgID)
        {

           var  members = repository.GetMany(m => m.CenterID == CenterID && m.IsActive==true && m.MemberStatus=="1" && m.OfficeID==officeId && m.OrgID==OrgID).OrderBy(mbr => mbr.MemberCode);
            return members;
        }

        public IEnumerable<Member> GetByOfficeId(int officeId, int OrgID)
        {
            var members = repository.GetMany(m => m.IsActive == true && m.MemberStatus == "1" && m.OfficeID == officeId && m.OrgID == OrgID).OrderBy(mbr => mbr.MemberCode);            
            return members;
        }
        //public Member GetByMemeberId(long Memberid)
        //{
        //    var members = repository.GetById(Memberid);
        //    return members;
        //}

        public Member GetByMemeberCode(string MemCode)
        {
            var members = repository.Get(g => g.MemberCode == MemCode && g.IsActive == true);
            return members;
        }

        public IEnumerable<DBMemberDetailModel> GetMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetMemberDetail(orgID,officeID, filterColumnName, filterValue, TypeFilterColumn, startRowIndex, jtSorting, pageSize, out TotCount);
        }
        public IEnumerable<DBMemberDetailModel> GetEligibleMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetEligibleMemberDetail(orgID,officeID, filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }
        public IEnumerable<DBMemberDetailModel> GetApprovalMember(int? orgID, int? officeID, out long TotCount)
        {
            return repository.GetApprovalMember(orgID,officeID, out TotCount);
        }
        public IEnumerable<DBMemberDetailModel> GetApprovedMemberForTransfer(int? orgID, int? officeID, out long TotCount)
        {
            return repository.GetApprovedMemberForTransfer(orgID, officeID, out TotCount);
        }
        public int GetTotalOrganizationMember()
        {
            return repository.GetTotalOrganizationMember();
        }


        public Member GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


       
        public IEnumerable<ValidationResult> CheckMemberNationalIdEdit(string nid, long mid)
        {
            var entity = repository.Get(p => (p.MemberStatus == "1" || p.MemberStatus=="0") && p.NationalID == nid && p.MemberID != mid);

            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Member", "Duplicate National Id Or Phone Number.");
            }
        }


        public IEnumerable<DBMemberDetailModel> GetMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount, short empID)
        {
            return repository.GetMemberDetail(orgID, officeID, filterColumnName, filterValue, TypeFilterColumn, startRowIndex, jtSorting, pageSize, out TotCount,empID);
        }


        public IEnumerable<DBMemberDetailModel> GetEligibleMemberDetail(int? orgID, int? officeID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount, short empID)
        {
            return repository.GetEligibleMemberDetail(orgID, officeID, filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount,empID);
        }


        public IEnumerable<ValidationResult> CheckMemberPhoneNoEdit(string nid, long mid)
        {
            var entity = repository.Get(p => p.MemberStatus == "1" && p.PhoneNo == nid && p.MemberID != mid);

            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Member", "Duplicate  Phone Number.");
            }
        }

        public IEnumerable<Member> GetMany(Expression<Func<Member, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValidationResult> CheckImageSize()
        {
            yield return new ValidationResult("Member", "File size must not exceed 100 KB.");
        }

       



    }
}
