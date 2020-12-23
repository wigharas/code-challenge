using System;

namespace challenge.Models
{
    public class Compensation
    {
        /// <summary>
        /// Gets or sets the compensation identifier.
        /// </summary>
        /// <value>
        /// The compensation identifier.
        /// </value>
        public string CompensationId { get; set; }
        
        /// <summary>
        /// The employee.
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// The employee salary.
        /// </summary>
        public double Salary { get; set; }


        /// <summary>
        /// The effective date.
        /// </summary>
        public DateTime EffectiveDate { get; set; }
    }
}
