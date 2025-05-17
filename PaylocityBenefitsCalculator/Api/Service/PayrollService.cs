using Api.Database;
using Api.Dtos.Employee;

namespace Api.Service
{
    public class PayrollService
    {
        #region constants

        /// <summary>
        /// Set of constants to easily update when requirements change.
        /// Should be done outside of the code in separate easily accessible config file to eliminate need to update the solution.
        /// </summary>
        private static readonly decimal cost = 1000m;
        private static readonly decimal costPerDependent = 600m;
        private static readonly decimal costPerDependent50 = 200m;
        // salaryTreshold is in %
        private static readonly decimal salaryTresholdPerc = 2;
        private static readonly decimal anual2PercLimit = 80000m;

        #endregion

        #region methods

        /// <summary>
        /// Method calculating monthly salary by substracting dynamic costs depending on dependents or anual salary.
        /// </summary>
        /// <param name="id">Id of employee we want to get salary for.</param>
        /// <param name="employee">Optional employee if the method is used for calculating bi-weekly salary.</param>
        /// <returns>Return monthly salary after substracting all valid costs. Value is rounded to 2 decimal places.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<decimal> GetMonthlySalary(int id, GetEmployeeDto? employee = null)
        {
            employee ??= await Data.GetEmployeeDto(id);

            // Check for missing data
            if (employee == null)
                throw new Exception($"Employee with ID {id} could not be found.");
            if (employee.Salary == 0)
                throw new Exception($"Salary for employee with ID {id} is 0.");

            int numberOfDependents = await employee.GetDependentsCount();
            int numberOfDependentsOver50 = await employee.GetDependentsOver50Count();

            // Divide anual salary by 12 to get monthly salary.
            decimal monthlySalary = employee.Salary / 12;

            // Substract cost from the monthly salary.
            decimal mSalMinusBase = monthlySalary - cost;

            // Substract cost per dependend.
            decimal mSalMinusDependants = mSalMinusBase - (numberOfDependents * costPerDependent);

            // Substract additional cost per dependent over 50 years of age.
            decimal mSalMinusDepOver50 = mSalMinusDependants - (numberOfDependentsOver50 * costPerDependent50);

            // Substract 1/12th of 2% of anual salary if anual salary is over specific treshhold.
            // This would be one of my points for clarification with the request creator.
            // I was not sure if this is done monthly or once per year (therefore the 1/12th).
            // I decided to go with the less harsh solution.
            decimal mSalMinusAnual2Perc = employee.Salary > anual2PercLimit ? mSalMinusDepOver50 - ((employee.Salary * (salaryTresholdPerc/100)) / 12) : mSalMinusDepOver50;

            return decimal.Round(mSalMinusAnual2Perc, 2);
        }

        /// <summary>
        /// Method calculating bi-weekly salary by multiplying monthly net salary by 12 to get net anual salary and dividing result by 26.
        /// </summary>
        /// <param name="id">Id of employee we want to get salary for.</param>
        /// <returns>Return bi-weekly salary after substracting all valid costs. Value is rounded to 2 decimal places.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<decimal> GetBiweeklySalary(int id)
        {
            GetEmployeeDto employee = await Data.GetEmployeeDto(id);

            // Check for missing data so we don't do useless operations.
            if (employee == null)
                throw new Exception($"Employee with ID {id} could not be found.");

            decimal monthlySalary = await GetMonthlySalary(id);

            // Multiply monthly salary to ge anual salary after all costs are substracted. Divide by 26 to get bi-weekly salary.
            decimal biweeklySalary = (monthlySalary * 12) / 26;

            return decimal.Round(biweeklySalary, 2);
        }

        #endregion
    }
}
