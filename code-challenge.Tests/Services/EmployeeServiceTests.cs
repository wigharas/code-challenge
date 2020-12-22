using challenge.Models;
using challenge.Repositories;
using challenge.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;

namespace code_challenge.Tests.Integration.Services
{
    [TestClass]
    public class EmployeeServiceTests
    {
        [TestMethod]
        public void GetEmployeeReportingStructure_Exceeds_MaxDepth()
        {
            
            //Arrange
            ILogger<EmployeeService> logger = Substitute.For<ILogger<EmployeeService>>();
            IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
            
            EmployeeService service = new EmployeeService(logger, repository);

            Employee employee1 = new Employee
            {
                EmployeeId = "fe69bd9b-8970-4856-bbfe-83cc461c2a24",
                FirstName = "Billy",
                LastName = "Corgan"
            };

            Employee employee2 = new Employee
            {
                EmployeeId = "cae3e8dc-6287-4aac-b4dc-b4383215c546",
                FirstName = "D'arcy",
                LastName = "Wretzky"
            };

            Employee employee3 = new Employee
            {
                EmployeeId = "964489b9-10dc-4c9c-a9a1-d85e99b11521",
                FirstName = "James",
                LastName = "Iha"
            };

            Employee employee4 = new Employee
            {
                EmployeeId = "9b3e10d9-2253-4465-9a6f-f7fa7793aa4c",
                FirstName = "Jimmy",
                LastName = "Chamberlain"
            };
            
            employee1.DirectReports = new List<Employee> {employee2};
            employee2.DirectReports = new List<Employee> {employee3};
            employee3.DirectReports = new List<Employee> {employee4};

            repository.GetById(Arg.Any<string>()).ReturnsForAnyArgs(employee1);
            
            //Act
            ReportingStructure actual = service.GetEmployeeReportingStructure(employee1.EmployeeId);

            //Assert
            Assert.IsNotNull(actual);
            
            //The NumberOfReports value will be the count under the maximum depth.
            //Since the graph has a depth of 4 and max depth is 3
            //The number of counts under the max depth is 3
            Assert.AreEqual(3, actual.NumberOfReports);
        }

    }
}