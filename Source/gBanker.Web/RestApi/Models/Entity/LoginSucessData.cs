using gBanker.Web.RestApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class LoginSucessData 
    {
        public List<Center> centers { get; set; }
        public List<Group> groups { get; set; }
        public List<MemberCategory> memberCategorys { get; set; }
        public List<Country> countrys { get; set; }
        public List<Division> divisions { get; set; }
        public List<District> districts { get; set; }
        public List<SubDistrict> subDistricts { get; set; }
        public List<Union> unions { get; set; }
        public List<Village> villages { get; set; }
        public List<Citizenship> citizenships { get; set; }
        
        public List<District> placeOfBirths { get; set; }
        public List<Gender> genders { get; set; }
        public List<HomeType> homeTypes { get; set; } 
        public List<GroupType> groupTypes { get; set; }
        public List<Education> educations { get; set; }
        public List<EconomicActivity> economicActivities { get; set; }
        public List<MaritalStatus> maritalStatuses { get; set; }
        public List<MemberType> memberTypes { get; set; }
        public List<Product> products { get; set; }
        public List<Investor> investors { get; set; }
        public List<Purpose> purposes { get; set; }
        public SelectListItem office { get; set; }
        public List<Bank> banks { get; set; }
        public List<Menu> menus { get; set; }
        public List<MenuPermission> permissions { get; set; }
        public string trxDate { get; set; }

        public void CitizenshipList()
        {
            citizenships =  new List<Citizenship>();
            //citizenships.Add(new Citizenship() { Text = "By Birth", Value = "BB", Selected = true });
            //citizenships.Add(new Citizenship() { Text = "Migrated", Value = "MI" });
            //citizenships.Add(new Citizenship() { Text = "Marital", Value = "MA" });
            //citizenships.Add(new Citizenship() { Text = "Nutralization", Value = "NU" });
        }
        public void GenderList()
        {
            genders = new List<Gender>();
            genders.Add(new Gender() { Text = "Male", Value = "Male" });
            genders.Add(new Gender() { Text = "Female", Value = "Female", Selected = "true" });
            genders.Add(new Gender() { Text = "Transgender", Value = "T" });
        }
        public void HomeTypeList()
        {
            homeTypes = new List<HomeType>();
            homeTypes.Add(new HomeType() { Text = "Building", Value = "BU" });
            homeTypes.Add(new HomeType() { Text = "Muddy", Value = "MU" });
            homeTypes.Add(new HomeType() { Text = "Rented", Value = "RE" });
            homeTypes.Add(new HomeType() { Text = "Semi Building", Value = "SB" });
            homeTypes.Add(new HomeType() { Text = "Tin Shade", Value = "TN", Selected = "true" });
        }
        public void GroupTypeList()
        {
            groupTypes = new List<GroupType>();
            groupTypes.Add(new GroupType() { Text = "Solidarity", Value = "SO", Selected = "true" });
            groupTypes.Add(new GroupType() { Text = "Non Solidarity", Value = "NS" });
            groupTypes.Add(new GroupType() { Text = "Individual", Value = "IN" });
            groupTypes.Add(new GroupType() { Text = "Corporate", Value = "CO" });
        }
        public void EducationList()
        {
            educations = new List<Education>();
            educations.Add(new Education() { Text = "Under Matric", Value = "UMA", Selected = "true" });
            educations.Add(new Education() { Text = "Pre-Primary", Value = "1" });
            educations.Add(new Education() { Text = "Primary", Value = "2" });
            educations.Add(new Education() { Text = "JSC", Value = "JSC" });
            educations.Add(new Education() { Text = "Secondary", Value = "3" });
            educations.Add(new Education() { Text = "Higher Secondary", Value = "4" });
            educations.Add(new Education() { Text = "Diploma", Value = "DIP" });
            educations.Add(new Education() { Text = "Graduate", Value = "5" });
            educations.Add(new Education() { Text = "PostGraduate", Value = "6" });
            educations.Add(new Education() { Text = "Illiterate", Value = "ILL" });
            educations.Add(new Education() { Text = "Other", Value = "7" });
        }
        public void EconomicActivityList()
        {
            economicActivities = new List<EconomicActivity>();
            economicActivities.Add(new EconomicActivity() { Text = "Business", Value = "BU", Selected = "true" });
            economicActivities.Add(new EconomicActivity() { Text = "House Hold", Value = "HH" });
            economicActivities.Add(new EconomicActivity() { Text = "Service", Value = "SE" });
            economicActivities.Add(new EconomicActivity() { Text = "Farmer", Value = "FR" });
        }
        public void MaritalStatusList()
        {
            maritalStatuses = new List<MaritalStatus>();
            maritalStatuses.Add(new MaritalStatus() { Text = "Business", Value = "BU", Selected = "true" });
            maritalStatuses.Add(new MaritalStatus() { Text = "House Hold", Value = "HH" });
            maritalStatuses.Add(new MaritalStatus() { Text = "Service", Value = "SE" });
            maritalStatuses.Add(new MaritalStatus() { Text = "Farmer", Value = "FR" });
        }
        public void MemberTypeListSpecfic()
        {
            memberTypes = new List<MemberType>();
            memberTypes.Add(new MemberType() { Text = "Member", Value = "1", Selected = "true" });
        }
        public void MemberTypeList()
        {
            memberTypes = new List<MemberType>();
            
            memberTypes.Add(new MemberType() { Text = "Member", Value = "1", Selected = "true" });
            memberTypes.Add(new MemberType() { Text = "Depositor", Value = "2" });
            memberTypes.Add(new MemberType() { Text = "Family", Value = "3" });
            memberTypes.Add(new MemberType() { Text = "Others", Value = "4" });
            memberTypes.Add(new MemberType() { Text = "Dormant", Value = "5" });
        }







    }
}