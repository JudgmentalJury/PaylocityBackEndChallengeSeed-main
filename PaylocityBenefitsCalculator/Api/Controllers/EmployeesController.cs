using Api.Database;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        GetEmployeeDto ?employee = await Data.GetEmployeeDto(id);

        if (employee != null)
        {
            return new ApiResponse<GetEmployeeDto>
            {
                Data = employee,
                Success = true
            };
        }
        else
            return new NotFoundResult();
    }

    /// <summary>
    /// I was thinking I will spin up local MSSQL DB connection here and use some simple commands to match production approach
    /// but I left it simple in case someone wants to run this code without bothering with local DB.
    /// </summary>
    /// <returns></returns>
    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //solved: use a more realistic production approach
        List<GetEmployeeDto> employees = await Data.GetAllEmployeeDtos();

        if (employees.Count > 0)
        {
            return new ApiResponse<List<GetEmployeeDto>>
            {
                Data = employees,
                Success = true
            };
        }
        else
            return new NotFoundResult();
    }
}
