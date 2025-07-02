
#region Using

using gBanker.Core.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Infrastructure.Html
{
    public static class DropdownListHelper
    {
        public static IEnumerable<SelectListItem> GetDropdownList(DropdownListTypes type, string selected = "")
        {
            var items = new List<ConstantDropdownItem>();

            switch (type)
            {
                case DropdownListTypes.AccChartLavel:
                    items = AccChartLavelConstants.Items.ToList();
                    break;
                case DropdownListTypes.EmploymentType:
                    items = EmploymentTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.EmploymentLoanCode:
                    items = EmploymentLoanCodeConstants.Items.ToList();
                    break;

                case DropdownListTypes.LoanSavingsRateType:
                    items = LoanSavingsRateTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.MonthList:
                    items = GetMonths().ToList();
                    break;
                case DropdownListTypes.ProcessType:
                    items = ProcessTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.SyncToPKSFType:
                    items = SyncToPKSFTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.NotSyncToPKSFType:
                    items = NotSyncToPKSFTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.MaleFemaleFlag:
                    items = MaleFemaleFlagConstants.Items.ToList();
                    break;
                case DropdownListTypes.PKSFCode:
                    items = MaleFemaleFlagConstants.Items.ToList();
                    break;
                case DropdownListTypes.EmploymentProduct:
                    items = EmploymentProductConstants.Items.ToList();
                    break;

            }

            return items.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = !string.IsNullOrWhiteSpace(selected) ? selected == i.Value : i.Selected
                }).ToList();
        }

        public static IEnumerable<ConstantDropdownItem> GetYears()
        {
            var years = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 15; i++)
            {
                var year = Convert.ToString(DateTime.Now.Year + i);
                years.Add(new ConstantDropdownItem
                {
                    Text = year,
                    Value = year,
                });
            }
            return years;
        }

        public static IEnumerable<ConstantDropdownItem> GetMonths()
        {
            var months = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 12; i++)
            {
                var text = (i < 10)
                                  ? "0" + i.ToString(CultureInfo.InvariantCulture)
                                  : i.ToString(CultureInfo.InvariantCulture);

                months.Add(new ConstantDropdownItem
                {
                    Text = text,
                    Value = i.ToString(CultureInfo.InvariantCulture),
                });
            }

            return months;
        }

        public static IEnumerable<ConstantDropdownItem> GetNumbersList()
        {
            var items = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 31; i++)
            {
                var text = i.ToString(CultureInfo.InvariantCulture);

                items.Add(new ConstantDropdownItem
                {
                    Text = text,
                    Value = text,
                });
            }

            return items;
        }
    }
}