using System;
using challenge.Data;
using challenge.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public class CompensationRepository: ICompensationRepository
    {
        private readonly EmployeeContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }    
        
        public Compensation GetById(string id)
        {
            _logger.LogDebug($"Retrieving compensation.Id:{id}");
            Compensation compensation =  _compensationContext.Compensations
                                            .SingleOrDefault(c => c.Employee.EmployeeId == id);
            return compensation;
        }

        public Compensation Add(Compensation compensation)
        {
            _logger.LogDebug($"Adding compensation.Id:{compensation.Employee.EmployeeId}");
            _compensationContext.Compensations.Add(compensation);
            return compensation;

        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
