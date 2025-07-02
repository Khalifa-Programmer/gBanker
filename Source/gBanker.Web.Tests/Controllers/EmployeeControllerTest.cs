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
using gBanker.Data.CodeFirstMigration.Db;
using AutoMapper;

namespace gBanker.Web.Tests.Controllers
{
    [TestClass]
    public class EmployeeControllerTest : TestBase
    {
        private EmployeeController _employeeController;
        [TestInitialize]
        public void SetupTests()
        {
            var employeeService = container.Resolve<IEmployeeService>();
            var officeService = container.Resolve<IOfficeService>();

            _employeeController = new EmployeeController(employeeService, officeService);
        }
        //[TestMethod]
        //public void GetEmployeeTest()
        //{
        //    Mapper.CreateMap<List<Employee>, List<EmployeeViewModel>>();
        //    var result = _employeeController.GetEmployee(0, 10, null);
        //    Assert.IsNotNull(result);
        //}
        [TestMethod]
        public void CreateTest()
        {
            Mapper.CreateMap<EmployeeViewModel, Employee>();
            var emp = new EmployeeViewModel() { EmployeeCode = Guid.NewGuid().ToString().Substring(0, 10), Designation = "TEST", EmpAddress = "TEST", BirthDate = DateTime.Now.AddYears(-30), EmpName = "Irfan test", Gender = "Male", OfficeID = 67, Email = "123test@loadtest.com" };
            var result = _employeeController.Create(emp);
            Assert.IsNotNull(result);
        }
    }
}
