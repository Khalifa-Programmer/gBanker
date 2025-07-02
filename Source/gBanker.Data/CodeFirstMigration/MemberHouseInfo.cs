namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberHouseInfo")]
    public partial class MemberHouseInfo
    {
        [Key]
        public long MemberHouseID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }

        public bool? HWPathkhari { get; set; }

        public bool? HWBash { get; set; }

        public bool? HWSchan { get; set; }

        public bool? HWPata { get; set; }

        public bool? HWMati { get; set; }

        public bool? HWTin { get; set; }

        public bool? HWOthers { get; set; }

        public bool? HRTin { get; set; }

        public bool? HRSchan { get; set; }

        public bool? HRKhar { get; set; }

        public bool? HRTinSchan { get; set; }

        public bool? HROthers { get; set; }


        public bool? HouseOwnerOwn { get; set; }
        public bool? HouseOwnerOther { get; set; }
        public bool? HouseOwnerKhasJomi { get; set; }
        public bool? HouseOwnerOtherLand { get; set; }

        public decimal? HouseOwnerOwnLandAmount { get; set; }
        public decimal? NoOfRoomInHouse { get; set; }
        public decimal? NoOfMainRoom { get; set; }

        public string OtherRoomName { get; set; }
        public string HouseMaterialTop { get; set; }
        public string HouseMaterialWall { get; set; }
        public string HouseMaterialFloor { get; set; }



        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
