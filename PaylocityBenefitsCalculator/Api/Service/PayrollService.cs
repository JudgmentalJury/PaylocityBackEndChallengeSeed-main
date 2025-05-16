using Api.Database;
using Api.Dtos.Employee;

namespace Api.Service
{
    public class PayrollService
    {
        // constants to easily update when requirements change
        private static readonly decimal cost = 1000m;
        private static readonly decimal costPerDependent = 600m;
        private static readonly decimal costPerDependent50 = 200m;
        private static readonly decimal costAbove80k = 0.02m;
        private static readonly decimal anual2PercLimit = 80000m;

        /// <summary>
        /// Method calculating bi-weekly salary by multiplying monthly net salary by 12 to get net anual salary and dividing result by 26.
        /// </summary>
        /// <param name="id">Id of employee we want to get salary for.</param>
        /// <returns>Returns decimal number rounded up to 2 decimal places.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<decimal> GetBiweeklySalary(int id)
        {
            GetEmployeeDto employee = await Data.GetEmployeeDto(id);

            // Check for missing data so we don't do useless operations.
            if (employee == null)
                throw new Exception($"Employee with ID {id} could not be found.");

            decimal monthlySalary = await GetMonthlySalary(id);
            decimal biweeklySalary = (monthlySalary * 12) / 26;

            return decimal.Round(biweeklySalary, 2);
        }

        /// <summary>
        /// Method calculating monthly salary by substracting dynamic costs depending on dependents or anual salary.
        /// </summary>
        /// <param name="id">Id of employee we want to get salary for.</param>
        /// <param name="employee">Optional employee if the method is used for calculating bi-weekly salary.</param>
        /// <returns>Returns decimal number rounded up to 2 decimal places.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<decimal> GetMonthlySalary(int id, GetEmployeeDto? employee = null)
        {
            employee ??= await Data.GetEmployeeDto(id);

            // Check for missing data so we don't do useless operations.
            if (employee == null)
                throw new Exception($"Employee with ID {id} could not be found.");
            if (employee.Salary == 0)
                throw new Exception($"Salary for employee with ID {id} is 0.");

            int numberOfDependents = await employee.GetDependentsCount();
            int numberOfDependentsOver50 = await employee.GetDependentsOver50Count();

            decimal monthlySalary = employee.Salary / 12;
            decimal mSalMinusBase = monthlySalary - cost;
            decimal mSalMinusDependants = mSalMinusBase - (numberOfDependents * costPerDependent);
            decimal mSalMinusDepOver50 = mSalMinusDependants - (numberOfDependentsOver50 * costPerDependent50);
            decimal mSalMinusAnual2Perc = employee.Salary > anual2PercLimit ? mSalMinusDepOver50 - ((employee.Salary * costAbove80k) / 12) : mSalMinusDepOver50;
            
            return decimal.Round(mSalMinusAnual2Perc, 2);
        }
    }
}
