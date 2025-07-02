using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class MemberCreateModel
    {
        public int id { get; set; }
        public int member_id { get; set; }
        public int center_id { get; set; }
        public int group_id { get; set; }
        public byte member_category_id { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string father_name { get; set; }
        public string mother_name { get; set; }
        public string marital_status { get; set; }
        public string spouse_name { get; set; }

        public string nid { get; set; }
        public string smart_card_no { get; set; }


        public int country_id { get; set; }
        public string division_id { get; set; }
        public string district_id { get; set; }
        public string sub_district_id { get; set; }
        public string union_id { get; set; }
        public string village_id { get; set; }
        public string present_address { get; set; }
        public string zip_code { get; set; }

        public int per_country_id { get; set; }
        public string per_division_id { get; set; }
        public string per_district_id { get; set; }
        public string per_sub_district_id { get; set; }
        public string per_union_id { get; set; }
        public string per_village_id { get; set; }
        public string permanent_address { get; set; }
        public string per_zip_code { get; set; }

        public int identity_type_id { get; set; }
        public string issue_date { get; set; }
        public string expire_date { get; set; }
        public string other_id_no { get; set; }
        public int provider_country_id { get; set; }

        public string date_of_birth { set; get; }
        public string age { get; set; }
        public string birth_place_id { get; set; }
        public string citizenship_id { get; set; }
        public string gender { get; set; }
        public string admission_date { get; set; }
        public string home_type { get; set; }
        public string group_type { get; set; }
        public string education_id { get; set; }
        public int family_member { get; set; }
        public string email { get; set; }
        public string contact_no { get; set; }
        public string family_contact_no { get; set; }
        public string reference_name { get; set; }
        public string co_applicant_name { get; set; }
        public string economic_activity_id { get; set; }
        public string total_wealth { get; set; }
        public byte member_type_id { get; set; }

        public string tin { get; set; }
        public string tax_amount { get; set; }
        public string member_status { get; set; }
        
        public bool is_any_fs { get; set; }
        public string f_service_name { get; set; }
        public int fin_service_choice_id { get; set; }
        public int transaction_choice_id { get; set; }

        public int office_id { get; set; }
        public int org_id { get; set; }
        public string created_user { get; set; }
        public string created_date { get; set; }

    }
}