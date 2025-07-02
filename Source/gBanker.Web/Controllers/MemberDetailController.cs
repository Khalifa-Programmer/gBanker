using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class MemberDetailController : BaseController
    {
        #region Variables
        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        private readonly IMemberFamilyInfoService memberFamilyInfoService;
        private readonly IMemberHouseInfoService memberHouseInfoService;
        private readonly IMemberLandInfoService memberLandInfoService;
        private readonly IMemberAssetInfoService memberAssetInfoService;
        private readonly IMemberOtherInfoService memberOtherInfoService;

        
        public MemberDetailController(IMemberService memberService, ICenterService centerService, IMemberFamilyInfoService memberFamilyInfoService, IMemberHouseInfoService memberHouseInfoService, IMemberLandInfoService memberLandInfoService, IMemberAssetInfoService memberAssetInfoService, IMemberOtherInfoService memberOtherInfoService)
        {
            this.memberService = memberService;
            this.centerService = centerService;
            this.memberFamilyInfoService = memberFamilyInfoService;
            this.memberHouseInfoService = memberHouseInfoService;
            this.memberLandInfoService = memberLandInfoService;
            this.memberAssetInfoService = memberAssetInfoService;
            this.memberOtherInfoService = memberOtherInfoService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(MemberDetailViewModel model)
        {
            var offc_id = Convert.ToInt32(LoginUserOfficeID);
            var allCenter = centerService.GetByOfficeId(offc_id,Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            center_items.AddRange(viewOffice);
            model.CenterList = center_items;

            var gender_item = new List<SelectListItem>();
            gender_item.Add(new SelectListItem() { Text = "Male", Value = "M" });
            gender_item.Add(new SelectListItem() { Text = "Female", Value = "F", Selected = true });
            model.GenderList = gender_item;

            var relationship_item = new List<SelectListItem>();
            relationship_item.Add(new SelectListItem() { Text = "Son", Value = "1", Selected = true });
            relationship_item.Add(new SelectListItem() { Text = "Husband", Value = "2"});
            relationship_item.Add(new SelectListItem() { Text = "Father", Value = "3" });
            relationship_item.Add(new SelectListItem() { Text = "Mother", Value = "4" });
            relationship_item.Add(new SelectListItem() { Text = "Brother", Value = "5" });
            relationship_item.Add(new SelectListItem() { Text = "Sister", Value = "6" });
            relationship_item.Add(new SelectListItem() { Text = "Uncle", Value = "7" });
           // relationship_item.Add(new SelectListItem() { Text = "Son", Value = "8" });
            relationship_item.Add(new SelectListItem() { Text = "Daughter", Value = "8" });
            model.FamilyMemRelationshipList = relationship_item;


            var MaritalStatus = new List<SelectListItem>();
            MaritalStatus.Add(new SelectListItem() { Text = "Married", Value = "M" });
            MaritalStatus.Add(new SelectListItem() { Text = "UnMarried", Value = "U", Selected = true });
            model.MaritalStatusList = MaritalStatus;
        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var memberList = new List<Member>();
            var mbr = memberService.GetByCenterId(Convert.ToInt32(centerId), Convert.ToInt32(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
            var members = mbr.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();
            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveFamily(string centerId, string memId, string memName, string memAge, string memGender, string memRelation, string memOccupation,string nationalIdNo,string mobileNo, string MaritalStatus, string FamilyMemOccupation2,string PhysicalDisability,string SocialSecurity, string letterWriting, string addressWriting, string finishClassFive, string dropBeforeClassFive, string studying, string signatureAbility, string mode, string FamilyEditId)
        {
            long mem_family_id = 0;
            bool PhyDis= false;
            bool chkletterWriting = false;
            bool chkaddressWriting = false;
            bool chkfinishClassFive = false;
            bool chkdropBeforeClassFive = false;
            bool chkstudying = false;
            bool chksignatureAbility = false;
            if (PhysicalDisability == "1")
                PhyDis = true;
            if (letterWriting == "1")
                chkletterWriting = true;
            if (addressWriting == "1")
                chkaddressWriting = true;
            if (finishClassFive == "1")
                chkfinishClassFive = true;
            if (dropBeforeClassFive == "1")
                chkdropBeforeClassFive = true;
            if (studying == "1")
                chkstudying = true;
            if (signatureAbility == "1")
                chksignatureAbility = true;
            if (mode == "S")
            {
                var mem_family = new MemberFamilyInfo() { 
                    OrgID = Convert.ToInt32(LoggedInOrganizationID), 
                    OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID),
                    CenterID = Convert.ToInt32(centerId),
                    MemberID = Convert.ToInt64(memId),
                    FamilyMemName = memName,
                    FamilyMemAge = Convert.ToInt32(memAge),
                    FamilyMemGender = memGender,
                    FamilyMemRelationship = memRelation,
                    FamilyMemOccupation = memOccupation,
                    LetterWritingAbility = chkletterWriting,
                    AddressWritingAbility = chkaddressWriting,
                    FinishedClassFive = chkfinishClassFive,
                    DropBeforeClassFive = chkdropBeforeClassFive,
                    Studying = chkstudying,
                    SignatureAbility = chksignatureAbility,
                    NationalIdNo = nationalIdNo,
                    MobileNo = mobileNo,
                    MaritalStatus = MaritalStatus,
                    FamilyMemOccupation2 = FamilyMemOccupation2,
                    PhysicalDisability = PhyDis,
                    SocialSecurity = SocialSecurity,
                    IsActive = true,
                    CreateUser = LoggedInEmployeeID.ToString(),
                    CreateDate = DateTime.Now
                };
                var familySave = memberFamilyInfoService.Create(mem_family);
                if (familySave.MemberFamilyID > 0)
                    mem_family_id = familySave.MemberFamilyID;
                else
                    mem_family_id = 0;
            }
            else if (mode == "U")
            {
                var family = memberFamilyInfoService.GetByFamilyInfoId(Convert.ToInt64(FamilyEditId));
                family.FamilyMemName = memName;
                family.FamilyMemAge = Convert.ToInt32(memAge);
                family.FamilyMemGender = memGender;
                family.FamilyMemRelationship = memRelation;
                family.FamilyMemOccupation = memOccupation;
                family.LetterWritingAbility = chkletterWriting;
                family.AddressWritingAbility = chkaddressWriting;
                family.FinishedClassFive = chkfinishClassFive;
                family.DropBeforeClassFive = chkdropBeforeClassFive;
                family.Studying = chkstudying;
                family.SignatureAbility = chksignatureAbility;
                family.NationalIdNo = nationalIdNo;
                family.MobileNo = mobileNo;
                family.MaritalStatus = MaritalStatus;
                family.FamilyMemOccupation2 = FamilyMemOccupation2;
                family.PhysicalDisability = PhyDis;
                family.SocialSecurity = SocialSecurity;
                memberFamilyInfoService.Update(family);
                mem_family_id = Convert.ToInt64(family.MemberFamilyID);
            }
            return Json(mem_family_id, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemFamilyInfo(int jtStartIndex, int jtPageSize, string jtSorting, long MemId)
        {
            try
            {
                //long TotCount;
                var memberDetail = memberFamilyInfoService.GetByFamilyInfoByMemberId(MemId);
                //Convert.ToInt16(LoggedInOrganizationID), SessionHelper.LoginUserOfficeID, filterColumn, filterValue, TypeFilterColumn, jtStartIndex, jtSorting, jtPageSize, out TotCount
                //var detail = memberDetail.ToList();
                var totCount = memberDetail.ToList().Count();
                var currentPageRecords = memberDetail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetMemHouseInfo(long MemId)
        {
            try
            {
                int cnt = 0;
                var house = memberHouseInfoService.GetByHouseInfoByMemberId(MemId);
                if (house != null)
                {
                    cnt = 1;
                    var result = new
                    {
                        TotCount = cnt,
                        MemberHouseID = house.MemberHouseID,
                        HomeWallPathkhari = house.HWPathkhari,
                        HomeWallBash = house.HWBash,
                        HomeWallSchan = house.HWSchan,
                        HomeWallPaka = house.HWPata,
                        HomeWallMati = house.HWMati,
                        HomeWallTin = house.HWTin,
                        HomeWallOther = house.HWOthers,
                        HomeRoofTin = house.HRTin,
                        HomeRoofSchan = house.HRSchan,
                        HomeRoofKhar = house.HRKhar,
                        HomeRoofTinSchan = house.HRTinSchan,
                        HomeRoofOther = house.HROthers,

                        HouseOwnerOwn = house.HouseOwnerOwn,
                        HouseOwnerOther = house.HouseOwnerOther,
                        HouseOwnerKhasJomi = house.HouseOwnerKhasJomi,
                        HouseOwnerOtherLand = house.HouseOwnerOtherLand,
                        HouseOwnerOwnLandAmount = house.HouseOwnerOwnLandAmount,
                        NoOfRoomInHouse = house.NoOfRoomInHouse,
                        NoOfMainRoom = house.NoOfMainRoom,
                        OtherRoomName = house.OtherRoomName,
                        HouseMaterialTop = house.HouseMaterialTop,
                        HouseMaterialWall = house.HouseMaterialWall,
                        HouseMaterialFloor = house.HouseMaterialFloor
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    cnt = 0;
                    var result = new
                    {
                        TotCount = cnt,
                        MemberHouseID = 0,
                        HomeWallPathkhari = false,
                        HomeWallBash = false,
                        HomeWallSchan = false,
                        HomeWallPaka = false,
                        HomeWallMati = false,
                        HomeWallTin = false,
                        HomeWallOther = false,
                        HomeRoofTin = false,
                        HomeRoofSchan = false,
                        HomeRoofKhar = false,
                        HomeRoofTinSchan = false,
                        HomeRoofOther = false,

                        HouseOwnerOwn = false,
                        HouseOwnerOther = false,
                        HouseOwnerKhasJomi = false,
                        HouseOwnerOtherLand = false,

                        HouseOwnerOwnLandAmount = 0,
                        NoOfRoomInHouse = 0,
                        NoOfMainRoom = 0,
                        OtherRoomName = "",
                        HouseMaterialTop = "",
                        HouseMaterialWall = "",
                        HouseMaterialFloor = "",


                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetMemHouseDelete(long HouseEditId)
        {
            var house = memberHouseInfoService.GetByHouseInfoId(HouseEditId);
            house.IsActive = false;
            house.InActiveDate = DateTime.Now;
            memberHouseInfoService.Update(house);
            var mem_house_id = Convert.ToInt64(house.MemberHouseID);            
            return Json(mem_house_id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveHouse(string centerId, string memId, string HomeWallPathkhari, string HomeWallBash, string HomeWallSchan, string HomeWallPaka, string HomeWallMati, string HomeWallTin, string HomeWallOther, string HomeRoofTin, string HomeRoofSchan, string HomeRoofKhar, string HomeRoofTinSchan,string HomeRoofOther, string HouseOwnerOwn, string HouseOwnerOther, string HouseOwnerKhasJomi, string HouseOwnerOtherLand, decimal? HouseOwnerOwnLandAmount, decimal? NoOfRoomInHouse, decimal? NoOfMainRoom, string OtherRoomName, string HouseMaterialTop, string HouseMaterialWall, string HouseMaterialFloor, string mode, string HouseEditId)
        {
            long mem_house_id = 0;
            bool chkHomeWallPathkhari = false;
            bool chkHomeWallBash = false;
            bool chkHomeWallSchan = false;
            bool chkHomeWallPaka = false;
            bool chkHomeWallMati = false;
            bool chkHomeWallTin = false;
            bool chkHomeWallOther = false;
            bool chkHomeRoofTin = false;
            bool chkHomeRoofSchan = false;
            bool chkHomeRoofKhar = false;
            bool chkHomeRoofTinSchan = false;
            bool chkHomeRoofOther = false;

            bool chkHouseOwnerOwn = false;
            bool chkHouseOwnerOther = false;
            bool chkHouseOwnerKhasJomi = false;
            bool chkHouseOwnerOtherLand = false;

            if (HomeWallPathkhari == "1")
                chkHomeWallPathkhari = true;
            if (HomeWallBash == "1")
                chkHomeWallBash = true;
            if (HomeWallSchan == "1")
                chkHomeWallSchan = true;
            if (HomeWallPaka == "1")
                chkHomeWallPaka = true;
            if (HomeWallMati == "1")
                chkHomeWallMati = true;
            if (HomeWallTin == "1")
                chkHomeWallTin = true;
            if (HomeWallOther == "1")
                chkHomeWallOther = true;
            if (HomeRoofTin == "1")
                chkHomeRoofTin = true;
            if (HomeRoofSchan == "1")
                chkHomeRoofSchan = true;
            if (HomeRoofKhar == "1")
                chkHomeRoofKhar = true;
            if (HomeRoofTinSchan == "1")
                chkHomeRoofTinSchan = true;
            if (HomeRoofOther == "1")
                chkHomeRoofOther = true;

            if (HouseOwnerOwn == "1")
                chkHouseOwnerOwn = true;
            if (HouseOwnerOther == "1")
                chkHouseOwnerOther = true;
            if (HouseOwnerKhasJomi == "1")
                chkHouseOwnerKhasJomi = true;
            if (HouseOwnerOtherLand == "1")
                chkHouseOwnerOtherLand = true;


            if (mode == "S")
            {
                var mem_house = new MemberHouseInfo()
                {
                    OrgID = Convert.ToInt32(LoggedInOrganizationID),
                    OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID),
                    CenterID = Convert.ToInt32(centerId),
                    MemberID = Convert.ToInt64(memId),
                    HWPathkhari = chkHomeWallPathkhari,
                    HWBash = chkHomeWallBash,
                    HWSchan = chkHomeWallSchan,
                    HWPata = chkHomeWallPaka,
                    HWMati = chkHomeWallMati,
                    HWTin = chkHomeWallTin,
                    HWOthers = chkHomeWallOther,
                    HRTin = chkHomeRoofTin,
                    HRSchan = chkHomeRoofSchan,
                    HRKhar = chkHomeRoofKhar,
                    HRTinSchan = chkHomeRoofTinSchan,
                    HROthers = chkHomeRoofOther,

                    HouseOwnerOwn = chkHouseOwnerOwn,
                    HouseOwnerOther = chkHouseOwnerOther,
                    HouseOwnerKhasJomi = chkHouseOwnerKhasJomi,
                    HouseOwnerOtherLand = chkHouseOwnerOtherLand,

                    HouseOwnerOwnLandAmount = HouseOwnerOwnLandAmount,
                    NoOfRoomInHouse = NoOfRoomInHouse,
                    NoOfMainRoom = NoOfMainRoom,
                    OtherRoomName = OtherRoomName,
                    HouseMaterialTop = HouseMaterialTop,
                    HouseMaterialWall = HouseMaterialWall,
                    HouseMaterialFloor = HouseMaterialFloor,

                    IsActive = true,
                    CreateUser = LoggedInEmployeeID.ToString(),
                    CreateDate = DateTime.Now
                };
                var houseSave = memberHouseInfoService.Create(mem_house);
                if (houseSave.MemberHouseID > 0)
                    mem_house_id = houseSave.MemberHouseID;
                else
                    mem_house_id = 0;
            }
            else if (mode == "U")
            {
                var house = memberHouseInfoService.GetByHouseInfoId(Convert.ToInt64(HouseEditId));
                    house.HWPathkhari = chkHomeWallPathkhari;
                    house.HWBash = chkHomeWallBash;
                    house.HWSchan = chkHomeWallSchan;
                    house.HWPata = chkHomeWallPaka;
                    house.HWMati = chkHomeWallMati;
                    house.HWTin = chkHomeWallTin;
                    house.HWOthers = chkHomeWallOther;
                    house.HRTin = chkHomeRoofTin;
                    house.HRSchan = chkHomeRoofSchan;
                    house.HRKhar = chkHomeRoofKhar;
                    house.HRTinSchan = chkHomeRoofTinSchan;
                    house.HROthers = chkHomeRoofOther;


                    house.HouseOwnerOwn = chkHouseOwnerOwn;
                    house.HouseOwnerOther = chkHouseOwnerOther;
                    house.HouseOwnerKhasJomi = chkHouseOwnerKhasJomi;
                    house.HouseOwnerOtherLand = chkHouseOwnerOtherLand;

                    house.HouseOwnerOwnLandAmount = HouseOwnerOwnLandAmount;
                    house.NoOfRoomInHouse = NoOfRoomInHouse;
                    house.NoOfMainRoom = NoOfMainRoom;
                    house.OtherRoomName = OtherRoomName;
                    house.HouseMaterialTop = HouseMaterialTop;
                    house.HouseMaterialWall = HouseMaterialWall;
                    house.HouseMaterialFloor = HouseMaterialFloor;

                memberHouseInfoService.Update(house);
                mem_house_id = Convert.ToInt64(house.MemberHouseID);
            }
            return Json(mem_house_id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMemFamilyDelete(long FamilyEditId)
        {
            var family = memberFamilyInfoService.GetByFamilyInfoId(FamilyEditId);
            family.IsActive = false;
            family.InActiveDate = DateTime.Now;
            memberFamilyInfoService.Update(family);
            var mem_family_id = Convert.ToInt64(family.MemberFamilyID);
            return Json(mem_family_id, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemLandInfo(long MemId)
        {
            try
            {
                int cnt = 0;
                var land = memberLandInfoService.GetByLandInfoByMemberId(MemId);
                if (land != null)
                {
                    cnt = 1;
                    var result = new
                    {
                        TotCount = cnt,
                        MemberLandID = land.MemberLandID,
                        LandVita = land.LandVita,
                        LandKrishi = land.LandKrishie,
                        LandBagan = land.LandBagan,
                        LandPukur = land.LandPukur,
                        LandOnabadi = land.LandOnabade,
                        LandBondhokDeya = land.LandBondhokDeya,
                        LandBorgaDeya = land.LandBorgaDeya,
                        LandBondhokNeya = land.LandBondhokNeya,
                        LandBorgaNeya = land.LandBorgaNeya,
                        LandKhash = land.KhashLand,
                        LandBorgaCondition = land.BorgaCondition  
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    cnt = 0;
                    var result = new
                    {
                        TotCount = cnt,
                        MemberLandID = 0,
                        LandVita = false,
                        LandKrishi = false,
                        LandBagan = false,
                        LandPukur = false,
                        LandOnabadi = false,
                        LandBondhokDeya = false,
                        LandBorgaDeya = false,
                        LandBondhokNeya = false,
                        LandBorgaNeya = false,
                        LandKhash = false,
                        LandBorgaCondition = string.Empty                        
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult SaveLand(string centerId, string memId, string LandVita, string LandKrishi, string LandBagan, string LandPukur, string LandOnabadi, string LandBondhokDeya, string LandBorgaDeya, string LandBondhokNeya, string LandBorgaNeya, string LandKhash, string LandBorgaCondition, string mode, string LandEditId)
        {
            long mem_land_id = 0;
            bool chkLandVita = false;
            bool chkLandKrishi = false;
            bool chkLandBagan = false;
            bool chkLandPukur = false;
            bool chkLandOnabadi = false;
            bool chkLandBondhokDeya = false;
            bool chkLandBorgaDeya = false;
            bool chkLandBondhokNeya = false;
            bool chkLandBorgaNeya = false;
            bool chkLandKhash = false;

            if (LandVita == "1")
                chkLandVita = true;
            if (LandKrishi == "1")
                chkLandKrishi = true;
            if (LandBagan == "1")
                chkLandBagan = true;
            if (LandPukur == "1")
                chkLandPukur = true;
            if (LandOnabadi == "1")
                chkLandOnabadi = true;
            if (LandBondhokDeya == "1")
                chkLandBondhokDeya = true;
            if (LandBorgaDeya == "1")
                chkLandBorgaDeya = true;
            if (LandBondhokNeya == "1")
                chkLandBondhokNeya = true;
            if (LandBorgaNeya == "1")
                chkLandBorgaNeya = true;
            if (LandKhash == "1")
                chkLandKhash = true;            
            if (mode == "S")
            {
                var mem_land = new MemberLandInfo()
                {
                    OrgID = Convert.ToInt32(LoggedInOrganizationID),
                    OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID),
                    CenterID = Convert.ToInt32(centerId),
                    MemberID = Convert.ToInt64(memId),
                    LandVita = chkLandVita,
                    LandKrishie = chkLandKrishi,
                    LandBagan = chkLandBagan,
                    LandPukur = chkLandPukur,
                    LandOnabade = chkLandOnabadi,
                    LandBondhokDeya = chkLandBondhokDeya,
                    LandBorgaDeya = chkLandBorgaDeya,
                    LandBondhokNeya = chkLandBondhokNeya,
                    LandBorgaNeya = chkLandBorgaNeya,
                    KhashLand = chkLandKhash,
                    BorgaCondition = LandBorgaCondition,                    
                    IsActive = true,
                    CreateUser = LoggedInEmployeeID.ToString(),
                    CreateDate = DateTime.Now
                };
                var landSave = memberLandInfoService.Create(mem_land);
                if (landSave.MemberLandID > 0)
                    mem_land_id = landSave.MemberLandID;
                else
                    mem_land_id = 0;
            }
            else if (mode == "U")
            {
                var land = memberLandInfoService.GetByLandInfoId(Convert.ToInt64(LandEditId));
                land.LandVita = chkLandVita;
                land.LandKrishie = chkLandKrishi;
                land.LandBagan = chkLandBagan;
                land.LandPukur = chkLandPukur;
                land.LandOnabade = chkLandOnabadi;
                land.LandBondhokDeya = chkLandBondhokDeya;
                land.LandBorgaDeya = chkLandBorgaDeya;
                land.LandBondhokNeya = chkLandBondhokNeya;
                land.LandBorgaNeya = chkLandBorgaNeya;
                land.KhashLand = chkLandKhash;
                land.BorgaCondition = LandBorgaCondition;

                memberLandInfoService.Update(land);
                mem_land_id = Convert.ToInt64(land.MemberLandID);
            }
            return Json(mem_land_id, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemLandDelete(long LandEditId)
        {
            var land = memberLandInfoService.GetByLandInfoId(LandEditId);
            land.IsActive = false;
            land.InActiveDate = DateTime.Now;
            memberLandInfoService.Update(land);
            var mem_house_id = Convert.ToInt64(land.MemberLandID);
            return Json(mem_house_id, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemAssetInfo(long MemId)
        {
            try
            {
                int cnt = 0;
                var asset = memberAssetInfoService.GetByAssetInfoByMemberId(MemId);
                if (asset != null)
                {
                    cnt = 1;
                    var result = new
                    {
                        TotCount = cnt,
                        MemberAssetID = asset.MemberAssetID,
                        AssetCow = asset.Cow,
                        AssetGoat = asset.Goat,
                        AssetSheep = asset.Sheep,
                        AssetDuck = asset.Duck,
                        AssetHen = asset.Hen,
                        AssetCattleOther = asset.Others,
                        AssetBed = asset.Bed,
                        AssetChair = asset.Chair,
                        AssetTable = asset.AssetTable,
                        AssetCycle = asset.Cycle,
                        AssetRadio = asset.Radio,
                        AssetOrnament = asset.Ornament,
                        AssetOther = asset.OtherAsset,

                        AgriculturalLandAmount = asset.AgriculturalLandAmount,
                        PondLandAmount = asset.PondLandAmount,
                        AbandonedLandAmount = asset.AbandonedLandAmount


                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    cnt = 0;
                    var result = new
                    {
                        TotCount = cnt,
                        MemberAssetID = 0,
                        AssetCow = string.Empty,
                        AssetGoat = string.Empty,
                        AssetSheep = string.Empty,
                        AssetDuck = string.Empty,
                        AssetHen = string.Empty,
                        AssetCattleOther = string.Empty,
                        AssetBed = string.Empty,
                        AssetChair = string.Empty,
                        AssetTable = string.Empty,
                        AssetCycle = string.Empty,
                        AssetRadio = string.Empty,
                        AssetOrnament = string.Empty,
                        AssetOther = string.Empty,

                        AgriculturalLandAmount = string.Empty,
                        PondLandAmount = string.Empty,
                        AbandonedLandAmount = string.Empty

                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult SaveAsset(string centerId, string memId, string AssetCow, string AssetGoat, string AssetSheep, string AssetDuck, string AssetHen, string AssetCattleOther, string AssetBed, string AssetChair, string AssetTable, string AssetCycle, string AssetRadio, string AssetOrnament, string AssetOther, decimal? AgriculturalLandAmount, decimal? PondLandAmount, decimal? AbandonedLandAmount, string mode, string AssetEditId)
        {
            long mem_asset_id = 0;
            if (mode == "S")
            {
                var mem_asset = new MemberAssetInfo()
                {
                    OrgID = Convert.ToInt32(LoggedInOrganizationID),
                    OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID),
                    CenterID = Convert.ToInt32(centerId),
                    MemberID = Convert.ToInt64(memId),
                    Cow = Convert.ToDecimal(string.IsNullOrEmpty(AssetCow) ? "0" : AssetCow),
                    Goat = Convert.ToDecimal(string.IsNullOrEmpty(AssetGoat) ? "0" : AssetGoat),
                    Sheep = Convert.ToDecimal(string.IsNullOrEmpty(AssetSheep) ? "0" : AssetSheep),
                    Duck = Convert.ToDecimal(string.IsNullOrEmpty(AssetDuck) ? "0" : AssetDuck),
                    Hen = Convert.ToDecimal(string.IsNullOrEmpty(AssetHen) ? "0" : AssetHen),
                    Others = AssetOther,
                    Bed = Convert.ToDecimal(string.IsNullOrEmpty(AssetBed) ? "0" : AssetBed),
                    Chair = Convert.ToDecimal(string.IsNullOrEmpty(AssetChair) ? "0" : AssetChair),
                    AssetTable = Convert.ToDecimal(string.IsNullOrEmpty(AssetTable) ? "0" : AssetTable),
                    Cycle = Convert.ToDecimal(string.IsNullOrEmpty(AssetCycle) ? "0" : AssetCycle),
                    Radio = Convert.ToDecimal(string.IsNullOrEmpty(AssetRadio) ? "0" : AssetRadio),
                    Ornament = Convert.ToDecimal(string.IsNullOrEmpty(AssetOrnament) ? "0" : AssetOrnament),
                    OtherAsset = AssetOther,
                    AgriculturalLandAmount = AgriculturalLandAmount,
                    PondLandAmount = PondLandAmount,
                    AbandonedLandAmount = AbandonedLandAmount,
                    IsActive = true,
                    CreateUser = LoggedInEmployeeID.ToString(),
                    CreateDate = DateTime.Now
                };
                var assetSave = memberAssetInfoService.Create(mem_asset);
                if (assetSave.MemberAssetID > 0)
                    mem_asset_id = assetSave.MemberAssetID;
                else
                    mem_asset_id = 0;
            }
            else if (mode == "U")
            {
                var asset = memberAssetInfoService.GetByAssetInfoId(Convert.ToInt64(AssetEditId));
                asset.Cow = Convert.ToDecimal(string.IsNullOrEmpty(AssetCow) ? "0" : AssetCow);
                asset.Goat = Convert.ToDecimal(string.IsNullOrEmpty(AssetGoat) ? "0" : AssetGoat);
                asset.Sheep = Convert.ToDecimal(string.IsNullOrEmpty(AssetSheep) ? "0" : AssetSheep);
                asset.Duck = Convert.ToDecimal(string.IsNullOrEmpty(AssetDuck) ? "0" : AssetDuck);
                asset.Hen = Convert.ToDecimal(string.IsNullOrEmpty(AssetHen) ? "0" : AssetHen);
                asset.Others = AssetOther;
                asset.Bed = Convert.ToDecimal(string.IsNullOrEmpty(AssetBed) ? "0" : AssetBed);
                asset.Chair = Convert.ToDecimal(string.IsNullOrEmpty(AssetChair) ? "0" : AssetChair);
                asset.AssetTable = Convert.ToDecimal(string.IsNullOrEmpty(AssetTable) ? "0" : AssetTable);
                asset.Cycle = Convert.ToDecimal(string.IsNullOrEmpty(AssetCycle) ? "0" : AssetCycle);
                asset.Radio = Convert.ToDecimal(string.IsNullOrEmpty(AssetRadio) ? "0" : AssetRadio);
                asset.Ornament = Convert.ToDecimal(string.IsNullOrEmpty(AssetOrnament) ? "0" : AssetOrnament);
                asset.OtherAsset = AssetOther;

                asset.AgriculturalLandAmount = AgriculturalLandAmount;
                asset.PondLandAmount = PondLandAmount;
                asset.AbandonedLandAmount = AbandonedLandAmount;

                memberAssetInfoService.Update(asset);
                mem_asset_id = Convert.ToInt64(asset.MemberAssetID);
            };
            return Json(mem_asset_id, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemAssetDelete(long AssetEditId)
        {
            var asset = memberAssetInfoService.GetByAssetInfoId(AssetEditId);
            asset.IsActive = false;
            asset.InActiveDate = DateTime.Now;
            memberAssetInfoService.Update(asset);
            var mem_house_id = Convert.ToInt64(asset.MemberAssetID);
            return Json(mem_house_id, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetOtherInfo(long MemId)
        {
            try
            {
                int cnt = 0;
                var Other = memberOtherInfoService.GetByOtherInfoByMemberId(MemId).FirstOrDefault();
                if (Other != null)
                {
                    cnt = 1;
                    var result = new
                    {
                        TotCount = cnt,
                        MemberOtherInfoID = Other.MemberOtherInfoID,
                        Tubewel = Other.Tubewel,
                        Nodi = Other.Nodi,
                        Khal = Other.Khal,
                        Pukur = Other.Pukur,
                        Filter = Other.Filter,
                        PSF = Other.PSF,
                        BristyrPani = Other.BristyrPani,
                        Saplai = Other.Saplai,
                        Others = Other.Others,
                        Lattin = Other.Lattin
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    cnt = 0;
                    var result = new
                    {
                        TotCount = cnt,
                        MemberOtherInfoID = 0,
                        Tubewel = false,
                        Nodi = false,
                        Khal = false,
                        Pukur = false,
                        Filter = false,
                        PSF = false,
                        BristyrPani = false,
                        Saplai = false,
                        Others = false,
                        Lattin = false
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult SaveOther(string centerId,
            string memId,
            string rdoTubewel,
            string rdoNodi,
            string rdoKhal,
            string rdoPukur,
            string rdoFilter,
            string rdoPSF,
            string rdoBristyrPani,
            string rdoSaplai,
            string rdoOthers,
            string rdolattinYes,
            string mode,
            string OtherEditId
        )
        {
            long mem_other_id = 0;

            bool rdoTubewelbool = false;
            if (rdoTubewel == "1")
                rdoTubewelbool = true;

            bool rdoNodibool = false;
            if (rdoNodi == "1")
                rdoNodibool = true;

            bool rdoKhalbool = false;
            if (rdoKhal == "1")
                rdoKhalbool = true;

            bool rdoPukurbool = false;
            if (rdoPukur == "1")
                rdoPukurbool = true;

            bool rdoFilterbool = false;
            if (rdoFilter == "1")
                rdoFilterbool = true;

            bool rdoPSFbool = false;
            if (rdoPSF == "1")
                rdoPSFbool = true;

            bool rdoBristyrPanibool = false;
            if (rdoBristyrPani == "1")
                rdoBristyrPanibool = true;

            bool rdoSaplaibool = false;
            if (rdoSaplai == "1")
                rdoSaplaibool = true;

            bool rdoOthersbool = false;
            if (rdoOthers == "1")
                rdoOthersbool = true;

            bool rdolattinYesbool = false;
            if (rdolattinYes == "1")
                rdolattinYesbool = true;

            if (mode == "S")
            {
                var mem_other = new MemberOtherInfo()
                {
                    OrgID = Convert.ToInt32(LoggedInOrganizationID),
                    OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID),
                    CenterID = Convert.ToInt32(centerId),
                    MemberID = Convert.ToInt64(memId),
                    Tubewel = rdoTubewelbool,
                    Nodi = rdoNodibool,
                    Khal = rdoKhalbool,
                    Pukur = rdoPukurbool,
                    Filter = rdoFilterbool,
                    PSF = rdoPSFbool,
                    BristyrPani = rdoBristyrPanibool,
                    Saplai = rdoSaplaibool,
                    Others = rdoOthersbool,
                    Lattin = rdolattinYesbool,
                    IsActive = true,
                    CreateUser = LoggedInEmployeeID.ToString(),
                    CreateDate = DateTime.Now
                };
                var otherSave = memberOtherInfoService.Create(mem_other);
                if (otherSave.MemberOtherInfoID > 0)
                    mem_other_id = otherSave.MemberOtherInfoID;
                else
                    mem_other_id = 0;
            }
            else if (mode == "U")
            {
                var other = memberOtherInfoService.GetByOtherInfoID(Convert.ToInt64(OtherEditId));
                other.Tubewel = rdoTubewelbool;
                other.Nodi = rdoNodibool;
                other.Khal = rdoKhalbool;
                other.Pukur = rdoPukurbool;
                other.Filter = rdoFilterbool;
                other.PSF = rdoPSFbool;
                other.BristyrPani = rdoBristyrPanibool;
                other.Saplai = rdoSaplaibool;
                other.Others = rdoOthersbool;
                other.Lattin = rdolattinYesbool;
                memberOtherInfoService.Update(other);
                mem_other_id = Convert.ToInt64(other.MemberOtherInfoID);
            };
            return Json(mem_other_id, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemOtherDelete(long OtherEditId)
        {
            var other = memberOtherInfoService.GetByOtherInfoID(OtherEditId);
            other.IsActive = false;
            other.InActiveDate = DateTime.Now;
            memberOtherInfoService.Update(other);
            var mem_other_id = Convert.ToInt64(other.MemberOtherInfoID);
            return Json(mem_other_id, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Events
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new MemberDetailViewModel();
            MapDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
