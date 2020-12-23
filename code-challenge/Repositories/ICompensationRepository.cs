using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        /// <summary>
        /// Gets the compensation by employee identifier.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <returns>
        /// The specified employee compensation.
        /// </returns>
        Compensation GetById(String id);

        /// <summary>
        /// Adds the specified employee compensation.
        /// </summary>
        /// <param name="compensation">The compensation.</param>
        /// <returns>
        /// The created compensation.
        /// </returns>
        Compensation Add(Compensation compensation);

        /// <summary>
        /// Saves the compensation asynchronous.
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }
}
