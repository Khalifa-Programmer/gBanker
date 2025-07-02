//using gBanker.Data.CodeFirstMigration;
//using gBanker.Service;
//using gBanker.Service.ReportServies;
//using gBanker.Web.Helpers;
//using gBanker.Web.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity.Validation;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace gBanker.Web.Controllers
//{
//    public class MemberNomineeOldController : BaseController
//    {
//        #region Variables
//        private readonly IMemberService memberService;
//        private readonly IMemberNomineeService memberNomineeService;
//        private readonly IGroupwiseReportService groupwiseReportService;
//        private readonly ICountryService countryService;
//        private readonly IUltimateReportService ultimateReportService;

//        public MemberNomineeOldController(
//            IMemberService memberService,
//            IMemberNomineeService memberNomineeService,
//            IGroupwiseReportService groupwiseReportService,
//            ICountryService countryService,
//            IUltimateReportService ultimateReportService
//        )
//        {
//            this.memberService = memberService;
//            this.memberNomineeService = memberNomineeService;
//            this.groupwiseReportService = groupwiseReportService;
//            this.countryService = countryService;
//            this.ultimateReportService = ultimateReportService;
//        }
//        #endregion


//        // GET: MemberNominee
//        //public ActionResult Create()
//        //{
//        //    var model = new MemberNomineeSavingViewModel();
//        //    MapDropDownList(model);
//        //    return View(model);
//        //}

//        public ActionResult Create(int? MemberId)
//        {
//            var EditMemberCode = string.Empty;
//            var model = new MemberNomineeSavingViewModel();
//            MapDropDownList(model);
//            if (MemberId != null && MemberId != 0) 
//            {
//                EditMemberCode = memberService.GetById(Convert.ToInt32(MemberId)).MemberCode;
//                ViewData["EditMemberCode"] = EditMemberCode;
//            }
//            return View(model);
//        }

//        private void MapDropDownList(MemberNomineeSavingViewModel model)
//        {
//            var allCountry = countryService.GetAll();
//            var viewCountry = allCountry.Select(x => x).ToList().Select(x => new SelectListItem
//            {
//                Value = x.CountryId.ToString(),
//                Text = x.CountryName.ToString(),
//                Selected = x.CountryId.ToString() == "18" ? true : false
//            });
//            var country_items = new List<SelectListItem>();
//            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
//            country_items.AddRange(viewCountry);
//            model.CountryList = country_items;

//            //blank division
//            var divisionList = new List<SelectListItem>();
//            divisionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
//            model.DivisionList = divisionList;

//            //blank district
//            var districtList = new List<SelectListItem>();
//            districtList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
//            model.DistrictList = districtList;

//            //blank upozilla
//            var upozillaList = new List<SelectListItem>();
//            upozillaList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
//            model.UpozillaList = upozillaList;

//            //blank Union
//            var unionList = new List<SelectListItem>();
//            unionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
//            model.UnionList = unionList;

//            //blank Village
//            var villageList = new List<SelectListItem>();
//            villageList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
//            model.VillageList = villageList;

//            //id Type
//            var idTypeList = new List<SelectListItem>();
//            idTypeList.Add(new SelectListItem() { Text = "NID", Value = "1", Selected = true });
//            idTypeList.Add(new SelectListItem() { Text = "Smart Card", Value = "2" });
//            idTypeList.Add(new SelectListItem() { Text = "Birth Certificate", Value = "3"});
//            idTypeList.Add(new SelectListItem() { Text = "Passport", Value = "4"});
//            idTypeList.Add(new SelectListItem() { Text = "Driving Licence", Value = "5"});
//            model.IdTypeList = idTypeList;
//        }

//        public JsonResult GetDivisionList(string country_id)
//        {
//            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
//            var param = new { SearchByCode = country_id, SearchBy = "con", SearchType = "div" };
//            var divList = ultimateReportService.GetLocationList(param);

//            List_MemberViewModel = divList.Tables[0].AsEnumerable()
//            .Select(row => new MemberViewModel
//            {
//                DivisionCode = row.Field<string>("DivisionCode"),
//                DivisionName = row.Field<string>("DivisionName")
//            }).ToList();

//            var viewDivision = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
//            {
//                Value = x.DivisionCode.ToString(),
//                Text = x.DivisionName.ToString()
//            });
//            var div_items = new List<SelectListItem>();
//            div_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
//            div_items.AddRange(viewDivision);
//            return Json(div_items, JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult GetDistrictList(string div_id)
//        {
//            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
//            var param = new { SearchByCode = div_id, SearchBy = "div", SearchType = "dis" };
//            var disList = ultimateReportService.GetLocationList(param);

//            List_MemberViewModel = disList.Tables[0].AsEnumerable()
//            .Select(row => new MemberViewModel
//            {
//                DistrictCode = row.Field<string>("DistrictCode"),
//                DistrictName = row.Field<string>("DistrictName")
//            }).ToList();

//            var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
//            {
//                Value = x.DistrictCode.ToString(),
//                Text = x.DistrictName.ToString()
//            });
//            var dis_items = new List<SelectListItem>();
//            dis_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
//            dis_items.AddRange(viewDist);
//            return Json(dis_items, JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult GetUpozillaList(string dis_id)
//        {
//            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
//            var param = new { SearchByCode = dis_id, SearchBy = "dis", SearchType = "upo" };
//            var upoList = ultimateReportService.GetLocationList(param);

//            List_MemberViewModel = upoList.Tables[0].AsEnumerable()
//            .Select(row => new MemberViewModel
//            {
//                UpozillaCode = row.Field<string>("UpozillaCode"),
//                UpozillaName = row.Field<string>("UpozillaName")
//            }).ToList();

//            var viewUpo = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
//            {
//                Value = x.UpozillaCode.ToString(),
//                Text = x.UpozillaName.ToString()
//            });
//            var upo_items = new List<SelectListItem>();
//            upo_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
//            upo_items.AddRange(viewUpo);
//            return Json(upo_items, JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult GetUnionList(string upo_id)
//        {
//            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
//            var param = new { SearchByCode = upo_id, SearchBy = "upo", SearchType = "uni" };
//            var uniList = ultimateReportService.GetLocationList(param);

//            List_MemberViewModel = uniList.Tables[0].AsEnumerable()
//            .Select(row => new MemberViewModel
//            {
//                UnionCode = row.Field<string>("UnionCode"),
//                UnionName = row.Field<string>("UnionName")
//            }).ToList();

//            var viewUni = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
//            {
//                Value = x.UnionCode.ToString(),
//                Text = x.UnionName.ToString()
//            });
//            var uni_items = new List<SelectListItem>();
//            uni_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
//            uni_items.AddRange(viewUni);
//            return Json(uni_items, JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult GetVillageList(string uni_id)
//        {
//            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
//            var param = new { SearchByCode = uni_id, SearchBy = "uni", SearchType = "vil" };
//            var vilList = ultimateReportService.GetLocationList(param);

//            List_MemberViewModel = vilList.Tables[0].AsEnumerable()
//            .Select(row => new MemberViewModel
//            {
//                VillageCode = row.Field<string>("VillageCode"),
//                VillageName = row.Field<string>("VillageName")
//            }).ToList();

//            var viewVil = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
//            {
//                Value = x.VillageCode.ToString(),
//                Text = x.VillageName.ToString()
//            });
//            var vil_items = new List<SelectListItem>();
//            vil_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
//            vil_items.AddRange(viewVil);
//            return Json(vil_items, JsonRequestBehavior.AllowGet);
//        }


//        public JsonResult GetMemInfoByCode(string member_Code)
//        {
//            var result = 0;
//            try
//            {
//                var param = new { MemberCode = member_Code };
//                var empList = groupwiseReportService.GetMemberInfoByMemberCode(param, "dbo.GetMemberInfoByMemberCode"); //SP_GetEmployeeInfo_ByEmployeeCode

//                var List_MemberViewModel = empList.Tables[0].AsEnumerable()
//                    .Select(row => new MemberNomineeSavingListViewModel
//                    {
//                        MemberId = row.Field<long>("MemberId"),
//                        MemberCode = row.Field<string>("MemberCode"),
//                        MemberName = row.Field<string>("MemberName"),
//                        OfficeID = row.Field<int>("OfficeID"),
//                        OfficeName = row.Field<string>("OfficeName"),
//                        CreateUser = row.Field<string>("CreateUser"),
//                        CreateDate = row.Field<DateTime>("CreateDate"),

//                        CountryID = row.Field<int?>("CountryID"),
//                        DivisionCode = row.Field<string>("DivisionCode"),
//                        DistrictCode = row.Field<string>("DistrictCode"),
//                        UpozillaCode = row.Field<string>("UpozillaCode"),
//                        UnionCode = row.Field<string>("UnionCode"),
//                        VillageCode = row.Field<string>("VillageCode"),
//                        ZipCode = row.Field<string>("ZipCode"),
//                        AddressLine1 = row.Field<string>("AddressLine1")
//                    }).ToList();

//                result = 1;
//                return Json(new { result = result, data = List_MemberViewModel.ToList() }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
//            }
//        }


//        [HttpPost]
//        public JsonResult SaveMemberNomineeInformation(MemberNomineeSavingViewModel memberNominee)
//        {
//            long result = 0;
//            var message = string.Empty;

//            var entity = new MemberNominee();
//            var savedEntity = new MemberNominee();

//            try
//            {
//                if (memberNominee.MemberNomineeId == 0)
//                {
//                    var isDuplicate = memberNomineeService.GetMany(p => p.IsActive == true && p.MemberId == memberNominee.MemberId && p.NomineeName.ToUpper() == memberNominee.NomineeName.ToUpper());

//                    if (isDuplicate.Any())
//                    {
//                        message = "Duplicate NomineeName, Save denied";
//                    }
//                    else
//                    {
//                        entity.MemberId = memberNominee.MemberId;
//                        entity.NomineeName = memberNominee.NomineeName;
//                        entity.NomineeFather = memberNominee.NomineeFather;
//                        entity.NomineeMother = memberNominee.NomineeMother;
//                        entity.NomineeHusbandWife = memberNominee.NomineeHusbandWife;
//                        entity.NomineeBirthdate = memberNominee.NomineeBirthdate;
//                        entity.NomineeMobileNo = memberNominee.NomineeMobileNo;
//                        entity.NomineeRelation = memberNominee.NomineeRelation;
//                        entity.NomineeNationalId = memberNominee.NomineeNationalId;
//                        entity.IdType = memberNominee.IdType;


//                        entity.CountryID = memberNominee.CountryID;
//                        entity.DivisionCode = memberNominee.DivisionCode;
//                        entity.DistrictCode = memberNominee.DistrictCode;
//                        entity.UpozillaCode = memberNominee.UpozillaCode;
//                        entity.UnionCode = memberNominee.UnionCode;
//                        entity.VillageCode = memberNominee.VillageCode;
//                        entity.ZipCode = memberNominee.ZipCode;
//                        entity.AddressLine1 = memberNominee.AddressLine1;


//                        entity.NomineeImage = memberNominee.NomineeImage;
//                        entity.NomineeSignatureImage = memberNominee.NomineeSignatureImage;
//                        entity.IsActive = true;
//                        entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
//                        entity.CreateDate = DateTime.UtcNow;

//                        savedEntity = memberNomineeService.Create(entity);
//                        message = "Save Successfull";
//                        result = savedEntity.MemberNomineeId;
//                    }
//                }
//                else
//                {
//                    //var isDuplicate =
//                    //        memberNomineeService.GetAll().Where(
//                    //       p => p.IsActive == true && p.NomineeName.ToUpper() == memberNominee.NomineeName.ToUpper() && p.MemberId == memberNominee.MemberId && p.MemberNomineeId != memberNominee.MemberNomineeId).ToList();

//                    var isDuplicate =
//                            memberNomineeService.GetMany(
//                           p => p.IsActive == true && p.NomineeName.ToUpper() == memberNominee.NomineeName.ToUpper() && p.MemberId == memberNominee.MemberId && p.MemberNomineeId != memberNominee.MemberNomineeId);

//                    if (isDuplicate.Any())
//                    {
//                        message = "Duplicate NomineeName, Update denied";
//                    }
//                    else
//                    {
//                        var entityUpdate = memberNomineeService.GetById(Convert.ToInt32(memberNominee.MemberNomineeId));
//                        entityUpdate.MemberId = memberNominee.MemberId;
//                        entityUpdate.MemberNomineeId = memberNominee.MemberNomineeId;
//                        entityUpdate.NomineeName = memberNominee.NomineeName;
//                        entityUpdate.NomineeFather = memberNominee.NomineeFather;
//                        entityUpdate.NomineeMother = memberNominee.NomineeMother;
//                        entityUpdate.NomineeHusbandWife = memberNominee.NomineeHusbandWife;
//                        entityUpdate.NomineeBirthdate = memberNominee.NomineeBirthdate;
//                        entityUpdate.NomineeMobileNo = memberNominee.NomineeMobileNo;
//                        entityUpdate.NomineeRelation = memberNominee.NomineeRelation;
//                        entityUpdate.NomineeNationalId = memberNominee.NomineeNationalId;
//                        entityUpdate.IdType = memberNominee.IdType;

//                        entityUpdate.CountryID = memberNominee.CountryID;
//                        entityUpdate.DivisionCode = memberNominee.DivisionCode;
//                        entityUpdate.DistrictCode = memberNominee.DistrictCode;
//                        entityUpdate.UpozillaCode = memberNominee.UpozillaCode;
//                        entityUpdate.UnionCode = memberNominee.UnionCode;
//                        entityUpdate.VillageCode = memberNominee.VillageCode;
//                        entityUpdate.ZipCode = memberNominee.ZipCode;
//                        entityUpdate.AddressLine1 = memberNominee.AddressLine1;


//                        entityUpdate.IsActive = true;
//                        entityUpdate.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
//                        entityUpdate.CreateDate = DateTime.UtcNow;
//                        memberNomineeService.Update(entityUpdate);

//                        message = "Update Successfull";
//                        result = memberNominee.MemberNomineeId;
//                    }
//                }
//            }

//            catch (DbEntityValidationException ex)
//            {
//                var errorMessages = ex.EntityValidationErrors
//                        .SelectMany(x => x.ValidationErrors)
//                        .Select(x => x.ErrorMessage);

//                var fullErrorMessage = string.Join("; ", errorMessages);
//                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
//                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
//            }

//            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
//        }


//        public JsonResult ListMemberNomineeInformation(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, int MemberId)


//        {
//            var param = new { MemberId = MemberId };
//            var memOffcDesigList = groupwiseReportService.GetMemberNomineeList(param, "dbo.SP_GetMemberNomineeList"); //SP_GetEmployeeNomineeList

//            var List_MemNomineeViewModel = memOffcDesigList.Tables[0].AsEnumerable()
//           .Select(row => new MemberNomineeSavingViewModel
//           {
//               SNO = row.Field<string>("SNO"),
//               MemberId = row.Field<long>("MemberId"),
//               MemberCode = row.Field<string>("MemberCode"),
//               MemberNomineeId = row.Field<long>("MemberNomineeId"),
//               NomineeName = row.Field<string>("NomineeName"),
//               NomineeFather = row.Field<string>("NomineeFather"),
//               NomineeMother = row.Field<string>("NomineeMother"),
//               NomineeHusbandWife = row.Field<string>("NomineeHusbandWife"),
//               NomineeBirthdate = row.Field<DateTime?>("NomineeBirthdate"),
//               NomineeBirthdateStr = row.Field<string>("NomineeBirthdateStr"),
//               NomineeMobileNo = row.Field<string>("NomineeMobileNo"),
//               NomineeRelation = row.Field<string>("NomineeRelation"),
//               IdType = row.Field<int?>("IdType"),
//               NomineeNationalId = row.Field<string>("NomineeNationalId"),


//               CountryID = row.Field<int?>("CountryID"),
//               DivisionCode = row.Field<string>("DivisionCode"),
//               DistrictCode = row.Field<string>("DistrictCode"),
//               UpozillaCode = row.Field<string>("UpozillaCode"),
//               UnionCode = row.Field<string>("UnionCode"),
//               VillageCode = row.Field<string>("VillageCode"),
//               ZipCode = row.Field<string>("ZipCode"),
//               AddressLine1 = row.Field<string>("AddressLine1"),

//               CreateUser = row.Field<string>("CreateUser"),
//               CreateDate = row.Field<DateTime>("CreateDate"),
//               IsActive = row.Field<bool>("IsActive"),
//               OfficeID = row.Field<int>("OfficeID"),
//               OfficeName = row.Field<string>("OfficeName")
//           }).ToList();
//            var currentPageRecords = List_MemNomineeViewModel.Skip(jtStartIndex).Take(jtPageSize);
//            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_MemNomineeViewModel.LongCount(), JsonRequestBehavior.AllowGet });
//        }

//        public JsonResult InformationDeleteNomineeInformation(int Id)
//        {
//            var result = 0;
//            var message = "";
//            try
//            {
//                var model = memberNomineeService.GetById(Id);
//                model.IsActive = false;
//                model.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
//                model.CreateDate = DateTime.UtcNow;
//                memberNomineeService.Update(model);
//                result = 1;
//                message = "Deleted Successfully";
//            }
//            catch (Exception)
//            {
//                result = 0;
//                message = "Delete Failed";

//            }
//            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

//        }


//        [HttpPost]
//        public ActionResult UploadNomineeImage(HttpPostedFileBase file, string MemberNomineeId)
//        {
//            var Result = 0;
//            var entity = memberNomineeService.GetByGurId(Convert.ToInt32(MemberNomineeId));

//            if (file != null)
//            {
//                byte[] data = new byte[file.ContentLength];
//                file.InputStream.Read(data, 0, file.ContentLength);
//                entity.NomineeImage = data;
//                memberNomineeService.Update(entity);
//                Result = 1;
//            }
//            else
//            {
//                Result = 2;
//            }
//            return Json(Result, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public ActionResult UploadNomineeSignatureImage(HttpPostedFileBase file, string MemberNomineeId)
//        {
//            var Result = 0;
//            var entity = memberNomineeService.GetByGurId(Convert.ToInt32(MemberNomineeId));

//            if (file != null)
//            {
//                byte[] data = new byte[file.ContentLength];
//                file.InputStream.Read(data, 0, file.ContentLength);
//                entity.NomineeSignatureImage = data;
//                memberNomineeService.Update(entity);
//                Result = 1;
//            }
//            else
//            {
//                Result = 2;
//            }
//            return Json(Result, JsonRequestBehavior.AllowGet);
//        }


//        public ActionResult RetrieveNomineeImage(int id)
//        {
//            byte[] cover = GetNomineeImageFromDataBase(id);
//            if (cover != null)
//            {
//                return File(cover, "image/*");
//            }
//            else
//            {
//                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
//                Image img = Image.FromFile(strImgPathAbsolute);
//                byte[] blnk;
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
//                    blnk = ms.ToArray();
//                }

//                return File(blnk, "image/*");
//            }
//        }
//        public byte[] GetNomineeImageFromDataBase(int Id)
//        {
//            var NomineeDetail = memberNomineeService.GetByGurId(Id);
//            var img = NomineeDetail.NomineeImage;
//            byte[] cover = img;
//            return cover;
//        }


//        public ActionResult RetrieveNomineeSignatureImage(int id)
//        {
//            byte[] cover = GetNomineeSignatureImageFromDataBase(id);
//            if (cover != null)
//            {
//                return File(cover, "image/*");
//            }
//            else
//            {
//                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
//                Image img = Image.FromFile(strImgPathAbsolute);
//                byte[] blnk;
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
//                    blnk = ms.ToArray();
//                }

//                return File(blnk, "image/*");
//            }
//        }
//        public byte[] GetNomineeSignatureImageFromDataBase(int Id)
//        {
//            var NomineeDetail = memberNomineeService.GetByGurId(Id);
//            var img = NomineeDetail.NomineeSignatureImage;
//            byte[] cover = img;
//            return cover;
//        }

//    }
//}