using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Database
{
    public static class Data
    {
        private static List<GetEmployeeDto> Employees = new()
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };

        public static async Task<List<GetEmployeeDto>> GetAllEmployeeDtos()
        {
            return Employees;
        }

        public static async Task<GetEmployeeDto> GetEmployeeDto(int employeeId)
        {
            return Employees.FirstOrDefault(x => x.Id == employeeId);
        }

        public static async Task<List<GetDependentDto>> GetAllDependentDtos()
        {
            return Employees.SelectMany(x => x.Dependents).ToList();
        }

        public static async Task<GetDependentDto> GetDependentDto(int dependentId)
        {
            return Employees.SelectMany(x => x.Dependents).FirstOrDefault(y => y.Id == dependentId);
        }

        public static async Task AddEmployeeDto(GetEmployeeDto employee)
        {
            Employees.Add(employee);
        }

        public static async Task AddDependentDto(int employeeId, GetDependentDto dependent)
        {
            GetEmployeeDto employee = await GetEmployeeDto(employeeId);

            if ((dependent.Relationship == Relationship.Spouse || dependent.Relationship == Relationship.DomesticPartner) && await EmployeeHasPartner(employeeId))
                throw new Exception($"Employee with ID {employeeId} already has a partner.");
            else
                employee.Dependents.Add(dependent); 
        }

        public static async Task<bool> EmployeeHasPartner(int employeeId)
        {
            GetEmployeeDto employee = await GetEmployeeDto(employeeId);
            return employee.Dependents.Select(x => x).Where(y => y.Relationship == Relationship.Spouse || y.Relationship == Relationship.DomesticPartner).ToList().Count > 0;
        }
    }
}