namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_SurveyMemberMaster
    {
        [Key]
        public int? RowSl { get; set; }
        public long SurveyId { get; set; }
        public string SurveyCode { get; set; }
        public DateTime SurveyDate { get; set; }
        public string MemberFullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentCountryId { get; set; }
        public string PermanentCountryId { get; set; }
        public string PresentAddressPOBCode { get; set; }
        public string PermanentAddressPOBCode { get; set; }
        public string RefereeName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PlaceOfBirth { get; set; }
        public int CityzenshipId { get; set; }
        public bool IsAnyRelationwithOtherNGO { get; set; }
        public string Education { get; set; }
        public string Ocupation { get; set; }
        public string MeritalStatus { get; set; }
        public bool IsActive { get; set; }


        public string ResidenceAddress { get; set; }
        public bool IsOwnHome { get; set; }

        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
