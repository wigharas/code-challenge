namespace challenge.Models
{
    public class ReportingStructure
    {

        /// <summary>
        /// Gets or sets the employee.
        /// </summary>
        /// <value>
        /// The employee.
        /// </value>
        public Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the number of direct reports under the employee.
        /// </summary>
        /// <value>
        /// The number of direct reports.
        /// </value>
        public int NumberOfReports { get; set; }
    }
}
