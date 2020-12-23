using challenge.Data;
using challenge.Models;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Gets the compensation by employee identifier.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <returns>
        /// The specified employee compensation.
        /// </returns>
        public Compensation GetById(string id)
        {
            _logger.LogDebug($"Retrieving compensation.Id:{id}");
            Compensation compensation =  _compensationContext.Compensations
                                            .Where(c => c.Employee.EmployeeId == id)
                                            .Include(e => e.Employee)                                            
                                            .ThenInclude( e => e.DirectReports)                                            
                                            .SingleOrDefault();
            return compensation;
        }

        /// <summary>
        /// Adds the specified employee compensation.
        /// </summary>
        /// <param name="compensation">The compensation.</param>
        /// <returns>
        /// The created compensation.
        /// </returns>
        public Compensation Add(Compensation compensation)
        {
            _logger.LogDebug($"Adding compensation.Id:{compensation.Employee.EmployeeId}");
            _compensationContext.Compensations.Add(compensation);
            return compensation;

        }

        /// <summary>
        /// Saves the compensation asynchronous.
        /// </summary>
        /// <returns></returns>
        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
