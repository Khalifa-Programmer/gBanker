using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Web.ViewModels
{
    public class SurveyViewModel
    {
        public Int64 MemberId { get; set; }

        public string PerAddressLine1 { get; set; }
        public long rowSl { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string SearchType { get; set; }
        public string MemberCode { get; set; }
        public string BirthDate { get; set; }
        
        public string MemberNameBng { get; set; }
        public string MemberNameEn { get; set; }
        public int MemberCategoryID { get; set; }
        public string AdmissionDate { get; set; }
        public string CenterName { get; set; }
        public int CenterID { get; set; }
        public int SurveyMemberTypeId { get; set; }
        public string MemberType { get; set; }
         
        public int CreateUser { get; set; }
        public int OccupationId { get; set; }
        public string Occupation { get; set; }

        public int GenderId { get; set; }
        public string Gender { get; set; }

        public int surveyIdentityTypeId { get; set; }
        public string IdentityName { get; set; }

        public int IdentityByCountryId { get; set; }
        public string IdentityByCountry { get; set; }

        public int MarriageStatusId { get; set; }
        public string MarriageType { get; set; }

        public int EducationStatusId { get; set; }
        public string EducationDegree { get; set; }

        public int HouseLocationId { get; set; }
        public string HouseLocation { get; set; }

        public int HouseTypeId { get; set; }
        public string HouseType { get; set; }
 
        public string CategoryName { get; set; }


        public string MemberNameBN { get; set; }  
         
        public string FatherNameBn { get; set; }
        public string FatherNameEn { get; set; }
        public string MotherNameBn { get; set; }  
        public string MotherNameEn { get; set; }  
        public string MaritialStatus { get; set; }
       
        public string SpouseNameBn   { get; set; }
        public string SpouseNameEn { get; set; }

        public string MemberOccupation { get; set; }


        public int MemberNationality             { get; set; }
        public int MemberIdentityType            { get; set; }
        //public int IdentityNumber                { get; set; }
        public string IdentityNumber            { get; set; }
        public string NationalID { get; set; }
        public string SmartCard { get; set; }


        /// <summary>
        /// ///Permanent Address
        /// </summary>
        /// 

        public int? PerCountryID { get; set; }
        public string PerDivisionCode { get; set; }
        public string PerDistrictCode { get; set; }
        public string PerUpozillaCode { get; set; }
        public string PerUnionCode { get; set; }
        public string PerVillageCode { get; set; }

        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string UpozillaCode { get; set; }
        public string UnionCode { get; set; }
        public string VillageCode { get; set; }



        public string PerCountryIDActu { get; set; }
        public string PerDivisionCodeActu { get; set; }
        public string PerDistrictCodeActu { get; set; }
        public string PerUpozillaCodeActu { get; set; }
        public string PerUnionCodeActu { get; set; }
        public string PerVillageCodeActu { get; set; }
        public string txtActuPostCode { get; set; }
        public string txtActuDakGhar { get; set; }
        public string ActuAddressLine1 { get; set; }
         
        public string IdentityValidationDate     { get; set; }
        public int IdentityProvidedByCountry     { get; set; }
        public int EducationalQualification      { get; set; }
        public string ContactNoSecond { get; set; }
        public string ContactNo                  { get; set; }
        public string ContactNoOnReq             { get; set; }
        public string HouseNo                    { get; set; }
        public string DakGhar                    { get; set; }
        public string Ward                       { get; set; }
        public string PostCode                   { get; set; }
        public string txtUnion                   { get; set; }
        public string txtThana                   { get; set; }
        public string txtTown                    { get; set; }
        public string txtCountry                 { get; set; }
        public string SurveyDate { get; set; }
        public int choiceId { get; set; }
        public string Choice { get; set; }

        public int ServiceUseReasonId { get; set; }
        public string ServiceUseReason { get; set; }

        public int transactionTypeId { get; set; }
        public string TransactionType { get; set; }


        public string TIN2 { get; set; }
        public string txtHouseNoRoadNo2 { get; set; }
        public string txtDakGhar2 { get; set; }
        public string txtWard2 { get; set; }
        public string txtPostCode2 { get; set; }
        public string txtUnion2 { get; set; }
        public string txtThanaUpozela2 { get; set; }
        public string txtTown2 { get; set; }
        public string txtCountry2 { get; set; }
        public string txtTaxAmount { get; set; }
        public string txtServiceName { get; set; }
        public int SurveyServiceUseReasonId { get; set; }
        public int SurveyTransactionType { get; set; }
        public string txtMemberrelativeInfo { get; set; }
        public string txtMemberReferencedBy { get; set; }

        public long SurveyEducationInfoId { get; set; }
         
        public string ChildrenEducationalInstitute { get; set; }
        public string ChildrenEducationalLevel { get; set; }
        public string ChildrenYearsOfStudy { get; set; }


        ////Family Info//


        public int SurveyFamilyId { get; set; }
        
 
public int MaleFamilyMember   { get; set; }
public int FemaleFamilyMember { get; set; }
public int TotalFamilyMember { get; set; }
        public string FamilyHeadName { get; set; }
        public string AnotherMFIMember { get; set; }
        public string MFIName { get; set; }
        public decimal MFIOfficalLoan { get; set; }
        public decimal UnofficialLoan { get; set; }
        public decimal MFISavings { get; set; }
        public decimal CoOperativeSavings { get; set; }
        public decimal BankSavings { get; set; }
        public decimal OtherSavings { get; set; }
        public int NumEarningMember { get; set; }
        public decimal FamilyMonthlyIncome { get; set; }
        public decimal FamilyMonthlyExpense { get; set; }
        public decimal HouseRent { get; set; }
        public decimal FoodCost { get; set; }
        public decimal EducationMedicalExpense { get; set; }
        public decimal OtherExpense { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal FamilyYearlyIncome { get; set; }
        public decimal FamilyYearlyExpense { get; set; }
        public string FamilyEarningSource { get; set; }
        public int NumChildrenStudying { get; set; }
        
        public string TotalLand { get; set; }
        public decimal TotalLandPrice { get; set; }
        public decimal FurniturePrice { get; set; }
        public decimal ElectronicsPrice { get; set; }
        public decimal ShopBusinessPrice { get; set; }
        public decimal OtherPropertyPrice { get; set; }
        public decimal OwnPropertyPrice { get; set; }
        public string BuroFamilyMemberName { get; set; }
        public int BuroFamilyMemberId { get; set; }
        public string BuroFamilyMemberRelation { get; set; }
        public decimal BuroFamilyMemberLoan { get; set; }


        //House Owner Info
       public string HouseOwnerName { get; set; }
        public string HouseOwnerPhoneNo { get; set; }
        public string HouseOwnerProfession { get; set; }
        //public decimal HouseRent { get; set; }
        public bool HouseRentPayment { get; set; }
        public string HouseRentalDuration { get; set; }

        /**Profession**/
        public string JobOrganizationName { get; set; }
        public string Designation { get; set; }
        public string JobOrganizationType { get; set; }
        public decimal MonthlySalary { get; set; }
        public string JobDuration { get; set; }
        public string JobTimeLeft { get; set; }
        public string JobOrgAddress { get; set; }
        public string JobOrgPostOfficeBox { get; set; }
        public string JobOrgWard { get; set; }
        public string JobOrgPostCode { get; set; }
        public string JobOrgUnion { get; set; }
        public string JobOrgThana { get; set; }
        public string JobOrgDistrict { get; set; }
        public string JobOrgCountry { get; set; }
        public string JobCoWorkerDetails { get; set; }


        public int? ddlComCountryID        { get; set; }
        public string ddlComDivisionCode     { get; set; }
        public string ddlComDistrictCode     { get; set; }
        public string ddlComUpozillaCode     { get; set; }
        public string ddlComUnionCode        { get; set; }
        public string ddlComVillageCode      { get; set; }
        public string txtComPostCode         { get; set; }
        public string txtComDakGhar          { get; set; }
        public string ComAddressLine1        { get; set; }



            /***/


            /** Investor Info **/

            public string BusinessOrgName { get; set; }
            public string BusinessType { get; set; }
            public string BusinessStartDatemsg { get; set; }// smalldatetime
            public string BusinessOrgAddress { get; set; }
            public string BusinessOrgPostOfficeBox { get; set; } 
            public string BusinessOrgWard { get; set; }
            public string BusinessOrgPostCode  { get; set; }
            public string BusinessOrgUnion { get; set; }
            public string BusinessOrgThana { get; set; }
            public string BusinessOrgDistrict  { get; set; }
            public string BusinessOrgCountry { get; set; }
            public decimal BusinessInvestment  { get; set; }  //decimal
            public decimal BusinessMonthlyIncome { get; set; } //decimal
            public decimal BusinessMonthlyExpense{ get; set; } //decimal
            public decimal BusinessMonthlyProfit { get; set; } //decimal
            public string NumBusinessEmployee { get; set; }
            public string TradeLicenseNo { get; set; }
            public string TradeLicenseExpireDatemsg { get; set; } //smalldatetime
            public string ShopRentContractExpireDatemsg { get; set; }//smalldatetime
            public string BankNameAndBranch { get; set; }
            public string AccountHeading  { get; set; }
            public string AccountNumber { get; set; }
            public string TIN { get; set; }
            public string CoBusinessWorkerDetails { get; set; }

        // Sadharon Tottho
        public string AnotherOrgLoan { get; set; }
        public decimal LoanAmount { get; set; }
        public string AnotherOrgName { get; set; }
        public string BusinessOnInterest { get; set; }
        public string LoanThroughTenant { get; set; }
        public string LoanPaymentRoutine { get; set; }

        public int? dofa { get; set; }
        public decimal? dofaloanAmount { get; set; }


        /// <summary>
        /// //// NOMINI
        /// </summary>
        /// 

        public string  NomineeName { get; set; }
       public string  NomineeFatherName { get; set; }
       public string  NomineeMotherName { get; set; }
       public string  NomineeSpouseName { get; set; }
       public string  NomineeNID { get; set; }
       public string  NomineeRelation { get; set; }
       public string  NomineeDateOfBirth { get; set; }
       public string  NomineeAddress { get; set; }
       public string  ApplicationDate { get; set; }



        public string ddlBizCountryID { get; set; }
        public string ddlBizDivisionCode { get; set; }
        public string ddlBizDistrictCode { get; set; }
        public string ddlBizUpozillaCode { get; set; }
        public string ddlBizUnionCode { get; set; }
        public string ddlBizVillageCode { get; set; }
        public string txtBizPostCode { get; set; }
        public string txtBizDakGhar { get; set; }
        public string BizAddressLine1 { get; set; }

















        public byte[] NomineePhotoBinary { get; set; }
        public string NomineePhotoBase64 { get; set; }
        public string ApplicantSignatureBase64 { get; set; }
        public byte[] ApplicantSignatureBinary { get; set; }

        public bool IsAnyFS { get; set; }

        public long ServicePhoneID { get; set; }
        public string ServiceName { get; set; }
        public string RegisterdPhoneNo { get; set; }

    }//END Class
}// END Namespace
 