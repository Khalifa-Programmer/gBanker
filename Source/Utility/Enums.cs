using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Enums
{
    public class DBEnums
    {
        public enum DataLoadingOption
        {
            LoadAll = 1,
            LoadWithPaged = 2,
            LoadWithFilterExpression = 3,
            LoadWithSortExpression = 4,
            LoadWithPagedAndFilterExpresson = 5,
            LoadWithPagedAndSortExpression = 6,
            LoadWithPagedAndFilterAndSortExpression = 7,
            LoadWithFilterAndSortExpression = 8,
            TotalRowCount = 9,
            LoadWithSpecialFilterExpression = 10,
            MaxRow=11,
            LoadRptGetValueWithFilterExpression = 12,
            LoaReportingTreeNameWithProjectCode=13,

        }
        public enum DbTransactionOption
        {
            Insert = 1,
            Update = 2,
            Delete = 3,
        }
        public enum DBTransactionStatus
        {
            Success,
            Fail,
            Aborted,
        }
        public enum VoucherStatus
        {
            Posted = 1,
            Unposted = 2,
            All = 3,
        }

        public enum ReportType
        {
            Comparitive = 1,
            Periodic = 2,
            
        }
        public enum DifferenceOn
        {
            Percentage = 1,
            Amount = 2,

        }

        public enum Validation
        {
            Required,
            Duplicate,
            LengthExceeds,
            MinimumLengthRequired,
            DataTypeMitchmatch,
            DateTimeOverflow,
            OverlappingDate,
            InvalidCharacter,            
            InvalidIDSupplied,
        }
        public enum StatusType
        {
            Active = 1,
            Inactive = 0,
        }
    }
}
