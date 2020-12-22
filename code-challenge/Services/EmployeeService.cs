using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        /// <summary>
        /// Gets the employee reporting structure.
        /// </summary>
        /// <remarks>
        /// If the employee DirectReport is null, 0 is assigned to NumberOfReports
        /// </remarks>
        /// <param name="employeeId">The employee identifier.</param>
        /// <returns>
        /// The employee reporting structure.
        /// </returns>
        public ReportingStructure GetEmployeeReportingStructure(string employeeId)
        {
            Employee employee = GetById(employeeId);

            if (employee == null)
                return null;

            int numberOfDirectReports = 0;
            CountDirectReports(employee, ref numberOfDirectReports, startingDepth:0, maximumDepth:3);

            ReportingStructure reportingStructure = new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = numberOfDirectReports
            };

            return reportingStructure;
        }

        /// <summary>
        /// Counts the number of direct employees under the current employee.
        /// This is a recursive function. Once the maximum depth has been exceeded recursion will stop
        /// and the count will be a number below the maximum depth.
        /// </summary>
        /// <param name="node">The employee node.</param>
        /// <param name="count">The number of direct employee count.</param>
        /// <param name="startingDepth">The starting depth.</param>
        /// <param name="maximumDepth">The maximum depth.</param>
        /// <remarks>
        /// The DirectReports contains a list of direct employees.
        /// </remarks>
        internal void CountDirectReports(Employee node, ref int count, int startingDepth, int maximumDepth)
        {
            //exclude root depth
            if (startingDepth > 0)
                count++;
            
            //stop recursion if depth max is reached
            if (startingDepth < maximumDepth)
            {
                if (node.DirectReports == null)
                    return;

                foreach (Employee emp in node.DirectReports)
                {
                    CountDirectReports(emp, ref count, startingDepth + 1, maximumDepth);
                }
            }

            _logger.LogWarning($"Maximum depth exceeded. Maximum Depth:{maximumDepth}");
        }
        
        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }
    }
}
