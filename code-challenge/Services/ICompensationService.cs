using challenge.Models;

namespace challenge.Services
{
    public interface ICompensationService
    {

        /// <summary>
        /// Creates the employee compensation.
        /// </summary>
        /// <param name="compensation">The compensation.</param>
        /// <returns>
        /// The created employee compensation
        /// </returns>
        Compensation CreateCompensation(Compensation compensation);

        /// <summary>
        /// Gets the compensation by employee identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Compensation GetById(string id);
    }
}
