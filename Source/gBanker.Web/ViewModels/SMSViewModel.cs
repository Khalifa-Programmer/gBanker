using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gBanker.Web.ViewModels
{
    public class SMSViewModel: BaseModel
    {
        public int SMSSetUpId { get; set; }
        public int MessageTypeId { get; set; }
        [Display(Name = "Message Type")]
        public string MessageType { get; set; }
        public long rowSl { get; set; }
        public int RowNum { get; set; }
        public long MemberId { get; set; }
        public long SummaryId { get; set; }
        public long RecordId { get; set; }
        public string TrxDate { get; set; }
        public string DisburseDate { get; set; }
        public int SMSMessageId { get; set; }
        public int CenterId { get; set; }
        public int MessageCategoryId { get; set; }

        public string MessageCategoryName { get; set; }
        public string MessageDetails { get; set; }
        public string PhoneNo { get; set; }
        public int Characters { get; set; }
        public int MessageSize { get; set; }
        public bool isActive { get; set; }
        public string MainItemName { get; set; }
        public string MainProductCode { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string OfficeCode { get; set; }
        public int SMSGroupId { get; set; }
        public string GroupName { get; set; }
        public string MemberCode { get; set; }

        public string SendTiming { get; set; }

        public string SMSPrice { get; set; }

        public int SendType { get; set; }

        public int Length { get; set; }
        public int SMSCount { get; set; }

    }// END lass
}// END NameSpace