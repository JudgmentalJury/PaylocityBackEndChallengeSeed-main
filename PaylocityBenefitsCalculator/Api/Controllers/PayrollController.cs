using Api.Models;
using Api.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class PayrollController : Controller
{
    private PayrollService? _payrollService;

    protected PayrollService PayrollService
    {
        get
        {
            _payrollService ??= new PayrollService();

            return _payrollService;
        }
    }

    /// <summary>
    /// Endpoint calculating bi-weekly salary for specific employee
    /// </summary>
    /// <returns></returns>
    [SwaggerOperation(Summary = "Get bi-weekly salary")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<decimal>>> Biweekly(int id)
    {
        try
        {
            decimal salary = await PayrollService.GetBiweeklySalary(id);

            return new ApiResponse<decimal>
            {
                Data = salary,
                Success = true
            };
        }
        catch(Exception ex)
        {
            return new NotFoundResult();
        }            
    }

    /// <summary>
    /// Endpoint calculating monthly salary for specific employee.
    /// 
    /// This is not a part of the requirements but it could be usefull and I already had the monthly calculation anyway.
    /// </summary>
    /// <returns></returns>
    [SwaggerOperation(Summary = "Get monthly salary")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<decimal>>> Monthly(int id)
    {
        try
        {
            decimal salary = await PayrollService.GetMonthlySalary(id);

            return new ApiResponse<decimal>
            {
                Data = salary,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new NotFoundResult();
        }
    }
}
