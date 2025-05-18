using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Api.Database;
using Azure;

namespace ApiTests.UnitTests
{
    public class EmployeeTest
    {
        [Fact]
        public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
        {
            List<GetEmployeeDto> employeeDtos = await Data.GetAllEmployeeDtos();
            Assert.Equal(4, employeeDtos.Count);
        }

        [Fact]
        public async Task WhenAskedForEmployee_ShouldReturnEmployee()
        {
            GetEmployeeDto employeeDto = await Data.GetEmployeeDto(2);
            Assert.Equal(new GetEmployeeDto
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
            }.ToString(), employeeDto.ToString());
        }

        [Fact]
        public async Task WhenAddingEmployee_ShouldAddEmployee()
        {
            List<GetEmployeeDto> employeeDtos = await Data.GetAllEmployeeDtos();
            int countBefore = employeeDtos.Count;

            GetEmployeeDto employee = new()
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
            };
            await Data.AddEmployeeDto(employee);

            employeeDtos = await Data.GetAllEmployeeDtos();
            Assert.Equal(countBefore+1, employeeDtos.Count);
        }

        [Fact]
        public async Task WhenAdding2ndPartnerDependent_ShouldThrowException()
        {
            GetDependentDto dependent = new()
            {
                Id = 42,
                FirstName = "Spouse",
                LastName = "James",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(2000, 6, 23)
            };

            await Data.AddDependentDto(1, dependent);
            var exception = Assert.ThrowsAsync<Exception>(async () => await Data.AddDependentDto(1, dependent));
            var exceptionResult = exception.Result;
            Assert.Equal("Employee with ID 1 already has a partner.", exceptionResult.Message);
        }
    }
}
