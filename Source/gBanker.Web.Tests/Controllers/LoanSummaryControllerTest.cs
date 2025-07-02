using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using gBanker.Web;
using gBanker.Web.Controllers;
using gBanker.Web.Tests.Dependencies;
using gBanker.Service;
using gBanker.Web.ViewModels;
using gBanker.Web.Helpers;
using gBanker.Data.DBDetailModels;
using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;

namespace gBanker.Web.Tests.Controllers
{
    [TestClass]
    public class LoanSummaryControllerTest : TestBase
    {
        private LoanSummaryController _controller;
        [TestInitialize]
        public void SetupTests()
        {

            ILoanSummaryService loansSummaryService = container.Resolve<ILoanSummaryService>();
            IProductService productService = container.Resolve<IProductService>();
            IMemberCategoryService membercategoryService = container.Resolve<IMemberCategoryService>();
            ICenterService centerService = container.Resolve<ICenterService>();
            IPurposeService purposeService = container.Resolve<IPurposeService>();
            IMemberService memberService = container.Resolve<IMemberService>();
            IInvestorService investorService = container.Resolve<IInvestorService>(); 

            var officeService = container.Resolve<IOfficeService>();

            _controller = new LoanSummaryController(loansSummaryService, productService, membercategoryService, officeService, centerService, purposeService, memberService, investorService);
        }

        [TestMethod]
        public void LoanSummaryCreateTest()
        {
            Mapper.CreateMap<LoanSummaryViewModel, LoanSummary>();
            // Act
            // string officeId = "67";
            var obj = new LoanSummaryViewModel()
            {
                OfficeID = 67,
                CenterID = 50,
                MemberCategoryID = 2               
            };
            var result = (JsonResult)_controller.Create(obj, null);
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
