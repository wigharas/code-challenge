using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));
                
            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateEmployee_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newEmployee = response.DeserializeContent<Employee>();
            Assert.IsNotNull(newEmployee.EmployeeId);
            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
            Assert.AreEqual(employee.Department, newEmployee.Department);
            Assert.AreEqual(employee.Position, newEmployee.Position);
        }

        [TestMethod]
        public void GetEmployeeById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var employee = response.DeserializeContent<Employee>();
            Assert.AreEqual(expectedFirstName, employee.FirstName);
            Assert.AreEqual(expectedLastName, employee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_Ok()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Department = "Engineering",
                FirstName = "Pete",
                LastName = "Best",
                Position = "Developer VI",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var putRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var putResponse = putRequestTask.Result;
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
            var newEmployee = putResponse.DeserializeContent<Employee>();

            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_NotFound()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "Invalid_Id",
                Department = "Music",
                FirstName = "Sunny",
                LastName = "Bono",
                Position = "Singer/Song Writer",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetEmployeeReportingStructure_Returns_NotFound()
        {
            //Arrange
            var employee = new Employee()
            {
                EmployeeId = "42722d57-26d9-44cf-9ffa-cc848ba8ff27",
                Department = "Area51",
                FirstName = "Jimmy",
                LastName = "Hoffa",
                Position = "Union Leader"
            };

            //Act
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employee.EmployeeId}/reporting-structure");
            var response = getRequestTask.Result;

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [TestMethod]
        public void GetEmployeeReportingStructure_4DirectReports_Returns_OK()
        {
            // Arrange
            var expectedEmployee = new Employee
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                FirstName = "John",
                LastName = "Lennon"
            };
            
            // Act
            var getReportingStructureRequestTask = _httpClient.GetAsync($"api/employee/{expectedEmployee.EmployeeId}/reporting-structure");
            var getReportingStructureResponse = getReportingStructureRequestTask.Result;
            var reportingStructure = getReportingStructureResponse.DeserializeContent<ReportingStructure>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, getReportingStructureResponse.StatusCode);
            Assert.AreEqual(expectedEmployee.FirstName, reportingStructure.Employee.FirstName);
            Assert.AreEqual(expectedEmployee.LastName, reportingStructure.Employee.LastName);
            Assert.AreEqual(4, reportingStructure.NumberOfReports);

        }

        [TestMethod]
        public void GetEmployeeReportingStructure_No_DirectReports_Returns_OK()
        {
            // Arrange
            var expectedEmployee = new Employee
            {
                EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                FirstName = "George",
                LastName = "Harrison"
            };

            // Act
            var getReportingStructureRequestTask = _httpClient.GetAsync($"api/employee/{expectedEmployee.EmployeeId}/reporting-structure");
            var getReportingStructureResponse = getReportingStructureRequestTask.Result;
            var reportingStructure = getReportingStructureResponse.DeserializeContent<ReportingStructure>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, getReportingStructureResponse.StatusCode);
            Assert.AreEqual(expectedEmployee.FirstName, reportingStructure.Employee.FirstName);
            Assert.AreEqual(expectedEmployee.LastName, reportingStructure.Employee.LastName);
            Assert.AreEqual(0, reportingStructure.NumberOfReports);
        }

    }
}
