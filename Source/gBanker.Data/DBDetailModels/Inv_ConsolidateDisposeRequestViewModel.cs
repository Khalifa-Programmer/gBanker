namespace gBanker.Data.DBDetailModels
{
    using System;

    public partial class Inv_ConsolidateDisposeRequestViewModel
    {
        public int ConsolidateDisposeID { get; set; }
        public int ItemID { get; set; }
        public int Qty { get; set; }
        public DateTime ConsolidateDate { get; set; }
        public int ConsolidateOfficeID { get; set; }
        public long? ConsolidateBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedOfficeID { get; set; }
        public long? ApprovedBy { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int? ConWarehouseID { get; set; }
        public string ConWarehouseName { get; set; }
    }
}

