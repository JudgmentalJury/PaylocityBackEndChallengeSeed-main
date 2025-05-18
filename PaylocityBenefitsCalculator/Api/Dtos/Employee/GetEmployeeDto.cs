using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public class GetEmployeeDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Salary { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();

    private readonly int yearsForSalaryDeduction = 50;

    public async Task<int> GetDependentsCount()
    {
        int countOfDep = 0;
        if (Dependents != null)
            countOfDep = Dependents.Count;

        return countOfDep;
    }

    public async Task<int> GetDependentsOver50Count()
    {
        int countOfDep = 0;
        if (Dependents != null)
            countOfDep = Dependents.Where(dep => dep.DateOfBirth < DateTime.Now.AddYears(-yearsForSalaryDeduction)).Count();

        return countOfDep;
    }
}
