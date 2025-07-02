using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.RestApi.Models.RequestModels;
using gBanker.Web.RestApi.Models.ResponseModels;
using gBanker.Web.RestApi.Models.Entity;
using gBanker.Service;
using System.Data.SqlClient;

namespace gBanker.Web.RestApi
{

    [System.Web.Mvc.RoutePrefix("/api/member")]
    public class MemberController : ApiController
    {
        protected IMemberService memberService;
        protected IGroupService groupService;

        public MemberController(IMemberService ms, IGroupService gs)
        {
            memberService = ms;
            groupService = gs;
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/new")]
        public HttpResponseMessage AddMember(MemberCreateModel model)
        {

            try
            {


                string groupSql = "SELECT TOP 1 GroupID FROM dbo.[Group] WHERE OfficeID=" + model.office_id + " AND IsActive=1 ORDER BY GroupID ASC";

                short groupID = new gBankerDbContext().Database.SqlQuery<short>(groupSql).FirstOrDefault();
                string _identityCheckParams = "'" + model.contact_no + "', 'phone'";
                IdentityCheckResult member = new gBankerDbContext().Database.SqlQuery<IdentityCheckResult>("Exec API_CheckMemberIdentity " + _identityCheckParams).FirstOrDefault();

                if (member != null)
                {
                    ErrorResponse er = new ErrorResponse();
                    er.status = "false";
                    er.message = "Sorry! Unable to create another member with same phone no ["+model.contact_no+"]";
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK,er, Configuration.Formatters.JsonFormatter);
                }

                LastMemberCode lastMemberCode = new gBankerDbContext().Database.SqlQuery<LastMemberCode>("Exec GenerateMemberLastCode " + model.office_id).FirstOrDefault();

                string _params = "'" + lastMemberCode.LastCode + "', " + model.office_id + "," + model.center_id + "," +
                                groupID + ",'" + model.admission_date + "','" + model.gender + "','" + model.nid + "'," +
                                model.member_category_id + "," + "0," + model.org_id + ",'" + model.created_user + "','" +
                                model.created_date + "','" + model.first_name + "','" + model.father_name + "','" +
                                model.mother_name + "','" + model.marital_status + "','" + model.spouse_name + "'," +
                                model.country_id + ",'" + model.division_id + "','" + model.district_id + "','" +
                                model.sub_district_id + "','" + model.union_id + "','" + model.village_id + "','" +
                                model.zip_code + "','" + model.present_address + "'," + model.per_country_id + ",'" +
                                model.per_division_id + "','" + model.per_district_id + "','" + model.per_sub_district_id + "','" +
                                model.per_union_id + "','" + model.per_village_id + "','" + model.per_zip_code + "','" +
                                model.permanent_address + "'," + model.identity_type_id + ",'" + model.smart_card_no + "','" +
                                model.issue_date + "','" + model.other_id_no + "','" + model.expire_date + "'," + model.provider_country_id + ",'" +
                                model.date_of_birth + "','" + model.age + "','" + model.birth_place_id + "','" + model.citizenship_id + "','" +
                                model.home_type + "','" + model.education_id + "'," + model.family_member + ",'" + model.email + "','" +
                                model.contact_no + "','" + model.family_contact_no + "','" + model.reference_name + "','" +
                                model.co_applicant_name + "','" + model.economic_activity_id + "'," + model.total_wealth + "," + model.member_type_id + ",'" +
                                model.tin + "'," + model.tax_amount + "," + model.is_any_fs + "," + model.fin_service_choice_id + ",'" +
                                model.f_service_name + "'," + model.transaction_choice_id;

                MemberCreateResult memberCreateResult = new gBankerDbContext().Database.SqlQuery<MemberCreateResult>("Exec API_MemberSave" + _params).FirstOrDefault();

                MemberCreateResponse response = new MemberCreateResponse();
                response.status = (memberCreateResult != null) ? "true" : "false";
                response.member = (memberCreateResult != null) ? memberCreateResult : null;
                response.message = (memberCreateResult != null) ? "Member successfully created" : "Sorry! Unable to create new member";

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);

            }catch(Exception ex)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, ex.Message, Configuration.Formatters.JsonFormatter);
            }
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/check-identity")]
        public HttpResponseMessage CheckMemberIdentity(IdentityCheckModel model)
        {
            string _params = "'"+model.keyword+"', '"+model.identityType+"'";
            IdentityCheckResult member = new gBankerDbContext().Database.SqlQuery<IdentityCheckResult>("Exec API_CheckMemberIdentity " + _params).FirstOrDefault();
            IdentityCheckResponseModel icrm = new IdentityCheckResponseModel();
            icrm.status = (member != null) ? "true" : "false";
            icrm.member = (member != null) ? member : null;
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, icrm, Configuration.Formatters.JsonFormatter);
        }


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpGet]
        [System.Web.Mvc.Route("/get-approval-products")]
        public HttpResponseMessage GetMemberProductsForApproval(GetApprovalProductRequest model)
        {
            MemberApproveProductResponse mapr = new MemberApproveProductResponse();
            
            mapr.products = new gBankerDbContext().Database.SqlQuery<GetApproveProduct>("Exec SP_GET_Member_ProdList_MemberCategoryWise " + model.orgId + "," + model.memberId).ToList();
            mapr.status = (mapr.products != null)?  "true" : "false";
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, mapr, Configuration.Formatters.JsonFormatter);
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/approve")]
        public HttpResponseMessage ApproveMember(ApproveMemberRequest requestModel)
        {
            //28,2904476,21,'18211',getDate()
            MemberApproveResponse response = new MemberApproveResponse();
            int executed =0;
            try {
                foreach (int pid in requestModel.products)
                {
                    string sql = "Exec API_Proc_Set_SavingOpeingWhenMemberEligible @OfficeID,@MemberID,@ProductID,@CreateUser,@CreateDate";
                    List<object> _params = new List<object>();
                    _params.Add(new SqlParameter("@OfficeID", requestModel.officeId));
                    _params.Add(new SqlParameter("@MemberID", requestModel.memberId));
                    _params.Add(new SqlParameter("@ProductID", pid));
                    _params.Add(new SqlParameter("@CreateUser", requestModel.createUser));
                    _params.Add(new SqlParameter("@CreateDate", requestModel.createDate));
                    object[] allparams = _params.ToArray();
                    executed = new gBankerDbContext().Database.ExecuteSqlCommand(sql, allparams);

                }
                response.status = "true";
                response.message = "Member approved successfully";
            }
            catch(Exception ex)
            {
                response.status = "false";
                response.message = "Member ["+requestModel.memberId+"] approval falied";
            }

            
           
            
            return Request.CreateResponse(System.Net.HttpStatusCode.OK,response, Configuration.Formatters.JsonFormatter);
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpGet]
        [System.Web.Mvc.Route("/list")]
        public HttpResponseMessage GetMembers(GetMemberListRequest model)
        {
            //CASE WHEN mpbr.MemberPassBookNO IS NULL Then 0 "+
            //"WHEN mpbr.MemberPassBookNO IS NOT NULL Then mpbr.MemberPassBookNO END AS MemberPassBookNo


            //string memberPassBookRegister = "(select MemberPassBookRegisterID from MemberPassBookRegister mpbr WHERE mpbr.IsActive  = 1 AND mpbr.Status =1 AND mpbr.MemberID  = m.MemberID ) MemberPassBookRegisterID";
            //string memberPassBookNO = "(select MemberPassBookNO from MemberPassBookRegister mpbr WHERE mpbr.IsActive  = 1 AND mpbr.Status =1 AND mpbr.MemberID  = m.MemberID ) MemberPassBookNo";
            //string sql = "SELECT " + memberPassBookRegister+","+memberPassBookNO + ", m.* FROM [Member] m ";
            //sql += " Inner JOIN Center c ON m.OfficeID = c.OfficeID AND m.CenterID = c.CenterID";
            //sql += " Inner JOIN Employee e on c.EmployeeID = e.EmployeeID";
            ////sql += " LEFT JOIN (Select * From MemberPassBookRegister Where IsActive=1 AND Status=1) mpbr ON mpbr.MemberID = m.MemberID";
            //sql +=    " WHERE m.OfficeID=" + model.officeId;

            //sql += " AND m.IsActive=1 AND m.MemberStatus IN (1,0)";


            //if(model.createUser != null)
            //{
            //    sql += " AND e.EmployeeCode='"+model.createUser+"' AND e.IsActive=1 AND e.EmployeeStatus=1";
            //}

            //var offset = (model.offset > 0) ? model.offset : 0;
            //var limit = (model.limit > 0) ? model.limit : 10;

            //sql +="  ORDER BY m.MemberID DESC OFFSET "+model.offset+" ROWS FETCH NEXT "+model.limit+" ROWS ONLY";

            string sql = "EXEC API_get_Members " + model.officeId + ",'" + model.createUser + "'";

            List<Models.Entity.Member> members = new gBankerDbContext().Database.SqlQuery<Models.Entity.Member>(sql).ToList();
            MemberListResponse mlr = new MemberListResponse();
            mlr.status = "true";
            members.ForEach((Models.Entity.Member m) =>
            {
                m.ImgSync = false;
                m.SigImgSync = false;

                if (m.MemberImg != null && m.MemberImg.Length>0)
                {
                    //m.MemberImg = null;
                    m.ImgSync = true;
                }

                if (m.ThumbImg != null && m.ThumbImg.Length>0)
                {
                    m.SigImgSync = true;
                }

                if (model.withoutImg)
                {
                    m.MemberImg = null;
                    m.ThumbImg = null;
                }
            });
            mlr.totalCount = members.Count;
            mlr.members = members;

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, mlr, Configuration.Formatters.JsonFormatter);
        }

        
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/sync-img")]
        public HttpResponseMessage SyncMemberImg(MemberImgSyncRequest model)
        {
            MemberImgSyncResponse imgSyncResponse = new MemberImgSyncResponse();
            imgSyncResponse.imgSync = false;
            imgSyncResponse.sigImgSync = false;

            try
            {
                int updated = 0;
                byte[] bytes = null;
                byte[] sigBytes = null;

                if (model.img != null && model.img != "")
                {
                    bytes = Convert.FromBase64String(model.img);
                }
                
                if(model.sigImg != null && model.sigImg != "")
                {
                    sigBytes = Convert.FromBase64String(model.sigImg);
                }
                
                using(gBankerDbContext db = new gBankerDbContext())
                {
                    long memberID = long.Parse(model.memberId.ToString());
                    var member = db.Members.First(x => x.MemberID == memberID);
                    if(bytes != null)
                    {
                        member.MemberImg = bytes;
                        imgSyncResponse.imgSync = true;
                    }

                    if(sigBytes != null)
                    {
                        member.ThumbImg = sigBytes;
                        imgSyncResponse.sigImgSync = true;
                        
                    }
                    
                    if(bytes != null || sigBytes != null)
                    {
                        updated = db.SaveChanges();
                        imgSyncResponse.status = "true";
                        imgSyncResponse.message = "Image synchornized successfully";
                    }
                    
                }

            }
            catch(Exception ex)
            {
                imgSyncResponse.status = "false";
                imgSyncResponse.message = ex.Message;
            }


            return Request.CreateResponse(System.Net.HttpStatusCode.OK, imgSyncResponse, Configuration.Formatters.JsonFormatter);
        }

     }


}