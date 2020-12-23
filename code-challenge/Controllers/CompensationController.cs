using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        
        public CompensationController(ILogger<CompensationService> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }    
        
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request.");

            Compensation newCompensation = _compensationService.CreateCompensation(compensation);

            if (newCompensation == null)
                return NotFound();
            
            return CreatedAtRoute("getCompensationById", new {id = newCompensation.Employee.EmployeeId}, newCompensation);
        }
        
        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(string id)
        {
            _logger.LogDebug($"Received compensation request. Id:{id}");

            var compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
