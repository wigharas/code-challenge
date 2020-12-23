using System;
using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;

namespace challenge.Services
{
    public class CompensationService: ICompensationService
    {

        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<CompensationService> _logger;
        
        public CompensationService(ILogger<CompensationService> logger, 
                                   ICompensationRepository compensationRepository, 
                                   IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _compensationRepository = compensationRepository;
            _employeeRepository = employeeRepository;
        }    

        public Compensation CreateCompensation(Compensation compensation)
        {
            if (compensation == null ||
                compensation.Employee == null)
                return null;
            
             _logger.LogDebug("Received creating compensation.");

             // Retrieve existing employee
             // If not found null is returned.
             Employee employee = _employeeRepository.GetById(compensation.Employee.EmployeeId);
             if (employee == null)
                 return null;

             _logger.LogDebug($"Creating Compensation. Employee Id:{employee.EmployeeId}");
             
             //Build compensation object
             Compensation newCompensation = new Compensation
             {
                 CompensationId = Guid.NewGuid().ToString(),
                 Employee = employee,
                 EffectiveDate = compensation.EffectiveDate,
                 Salary = compensation.Salary
             };    
             
             // Save
             _compensationRepository.Add(newCompensation);
             _compensationRepository.SaveAsync().Wait();
             _logger.LogDebug($"Compensation Saved. Id:{newCompensation.CompensationId}");

             return compensation;
        }

        public Compensation GetById(string id)
        {
            _logger.LogDebug($"Retrieving compensation. Id:{id}");
            if(!string.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetById(id);
            }

            _logger.LogDebug($"Unable to find compensation. Id:{id}");
            return null;
        }
    }
}
