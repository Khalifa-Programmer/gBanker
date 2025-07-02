using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
   public class Inv_ItemViewModel
    {
        public int ItemID { get; set; }
        public int? CategoryID { get; set; }
        public int? SubCatagoryID { get; set; }
        public string ItemName { get; set; }
        public string ItemNameInBangle { get; set; }
        public string ItemShortName { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public string ItemDetails { get; set; }
        public bool? IsActive { get; set; }
        public int MinStockLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public int ReOrderLevel { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal UnitPrice { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
    }
    public class Inv_SubcategoryViewModel
    {
        public int CategorySubCategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string CateorSubCateCode { get; set; }
        public string CategorySubCategoryName { get; set; }
        public string NameInBangla { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class Inv_WarehouseViewModel
    {
        public int StoreItemID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public string WarehouseName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }

    public class Inv_StoreViewModel
    {
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string Category { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public string OrganizationName { get; set; }
        public string OrgAddress { get; set; }
        public Byte[] OrgLOGO { get; set; }

    }
    public class Inv_StoreItemViewModel
    {
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
      
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int Qty { get; set; }
    }

    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
    public class InvUser
    {
        public string UID { get; set; }
        public int RoleID { get; set; }
        public int OfficeID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsDepartment { get; set; }
    }
    public class InvUserViewModel
    {
        public string UID { get; set; }
        public int RoleID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int DepartmentID { get; set; }
        public bool IsDepartment { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class RequsitionDetailViewmodel
    {
        public Int64 RequsitionDetailsID { get; set; }
        public Int64 RequsitionMasterID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public string Remarks { get; set; }
        public int ApprovedQty { get; set; }
        public string AprovedStatus { get; set; }
    }

    public class RequsitionAnalysisViewModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int Qty { get; set; }
        public int RequestQty { get; set; }
        public int ApprovedQty { get; set; }
        public int ModifyQty { get; set; }
        public int StockBalance { get; set; }
        public int MinStockLevel { get; set; }
    }

    public class ItemwiseStoreViewModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long ID { get; set; }
        public int StockBalance { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ItemXDisposeViewModel
    {
        public int ItemID { get; set; }
        public DateTime Date { get; set; }
        public int Qty { get; set; }
        public string StoreNo { get; set; }
    }
}
