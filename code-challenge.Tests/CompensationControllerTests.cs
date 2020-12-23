using challenge.Models;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using code_challenge.Tests.Integration.Extensions;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
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
        public void CreateCompensation_EmployeeExists_Created()
        {
            // Arrange
            var employee = new Employee
            {
                EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c"
            };

            DateTime utcEffectiveDateTime = new DateTime(2020, 12, 23, 10, 50, 30);
            Compensation compensation = new Compensation
            {
                Employee = employee,
                EffectiveDate = utcEffectiveDateTime.ToUniversalTime(),
                Salary = 115000
            };
            var postRequestContent = new JsonSerialization().ToJson(compensation);

            // Act
            var postRequestTask = _httpClient.PostAsync("api/compensation",
                new StringContent(postRequestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(newCompensation.Employee.EmployeeId, employee.EmployeeId);
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.AreEqual(newCompensation.EffectiveDate, compensation.EffectiveDate);
            Assert.AreEqual(newCompensation.Salary, compensation.Salary);
        }

        [TestMethod]
        public void CreateCompensation_EmployeeDoesExists_Return_NotFound()
        {
            // Arrange
            var employee = new Employee
            {
                EmployeeId = "badf00d1-16bd-4603-8e08-638a9d18b22c"
            };

            DateTime utcEffectiveDateTime = new DateTime(2020, 12, 23, 10, 50, 30);
            Compensation compensation = new Compensation
            {
                Employee = employee,
                EffectiveDate = utcEffectiveDateTime.ToUniversalTime(),
                Salary = 115000
            };
            var postRequestContent = new JsonSerialization().ToJson(compensation);

            // Act
            var postRequestTask = _httpClient.PostAsync("api/compensation",
                new StringContent(postRequestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void CreateCompensation_NullCompensation_Return_NotFound()
        {
            // Arrange
            var postRequestContent = "";

            // Act
            var postRequestTask = _httpClient.PostAsync("api/compensation",
                new StringContent(postRequestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void CreateCompensation_EmptyCompensation_Return_NotFound()
        {
            // Arrange
            var postRequestContent = "{}";

            // Act
            var postRequestTask = _httpClient.PostAsync("api/compensation",
                new StringContent(postRequestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
