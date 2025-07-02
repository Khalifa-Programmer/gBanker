namespace gBanker.Web.ViewModels
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RequsitionDisposeViewModel
    {
        public long DisposeRequestID { get; set; }
        public string DisposeRequestNo { get; set; }
        public int ItemID { get; set; }
        public int? Qty { get; set; }
        public decimal? UnitPrice { get; set; }
        public long? RequestBy { get; set; }
        public int? DisposeRequestOfficeID { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestRemark { get; set; }
        public long? RequestApprovedBy { get; set; }
        public int? DisposeRequestApproveOfficeID { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public long? DisposeBy { get; set; }
        public int? DisposeOfficeID { get; set; }
        public DateTime? DisposeDate { get; set; }
        public long? RejectBy { get; set; }
        public int? RejectOfficeID { get; set; }
        public DateTime? RejectDate { get; set; }
        public string Remark { get; set; }
        public int? DisposeStatus { get; set; }
        public string WarehouseName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }
}

