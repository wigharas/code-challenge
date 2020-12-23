using challenge.Models;
using challenge.Repositories;
using challenge.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace code_challenge.Tests.Integration.Services
{
    [TestClass]
    public class CompensationServiceTests
    {
        private ILogger<CompensationService> _logger;
        private CompensationService _compensationService;
        private ICompensationRepository _compensationRepository;
        private IEmployeeRepository _employeeRepository;
        
        [TestInitialize]
        public void Init()
        {
            
            _logger = Substitute.For<ILogger<CompensationService>>();
            _compensationRepository = Substitute.For<ICompensationRepository>();
            _employeeRepository = Substitute.For<IEmployeeRepository>();
            
            _compensationService = new CompensationService(_logger,_compensationRepository, _employeeRepository);
        }
        
        [TestMethod]
        public void GetById_EmptyId_Return_Null()
        {
            // Arrange
            // Act
            Compensation actual = _compensationService.GetById(string.Empty);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetById_NullId_Return_Null()
        {
            // Arrange
            // Act
            Compensation actual = _compensationService.GetById(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetById_Return_Compensation()
        {
            // Arrange
            Employee testEmployee = new Employee
            {
                EmployeeId = "C32A28E2-2311-4F5E-B024-7D2E43F3FBB7",
                FirstName = "Billy",
                LastName = "Corgan",
                Department = "Engineering",
                Position = "Intern",
                DirectReports = new List<Employee>()
            };

            DateTime localDateTime = new DateTime(2020, 12, 23, 16, 36, 0);
            Compensation testCompensation = new Compensation
            {
                Employee = testEmployee,
                EffectiveDate = localDateTime.ToUniversalTime(),
                Salary = 40000
            };
            _compensationRepository.GetById("C32A28E2-2311-4F5E-B024-7D2E43F3FBB7").Returns(testCompensation);

            //Act
            Compensation actual = _compensationService.GetById("C32A28E2-2311-4F5E-B024-7D2E43F3FBB7");

            //Assert
            Assert.AreEqual(testCompensation.Salary, actual.Salary);
            Assert.AreEqual(testCompensation.EffectiveDate, actual.EffectiveDate);
            Assert.AreEqual(testCompensation.Employee.EmployeeId, actual.Employee.EmployeeId);
            Assert.AreEqual(testCompensation.Employee.FirstName, actual.Employee.FirstName);
            Assert.AreEqual(testCompensation.Employee.LastName, actual.Employee.LastName);
            Assert.AreEqual(testCompensation.Employee.Department, actual.Employee.Department);
            Assert.AreEqual(testCompensation.Employee.Position, actual.Employee.Position);
            Assert.IsNotNull(actual.Employee.DirectReports);
        }
        
    }
}
