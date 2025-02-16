﻿using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(String id);
        Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);

        /// <summary>
        /// Gets the employee reporting structure.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <returns>
        /// The employee reporting structure.
        /// </returns>
        ReportingStructure GetEmployeeReportingStructure(string employeeId);

    }
}
